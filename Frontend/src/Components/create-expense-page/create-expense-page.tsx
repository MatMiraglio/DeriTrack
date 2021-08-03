import { Grid, Typography, TextField, Button, MenuItem } from "@material-ui/core"
import { Form, Formik } from "formik"
import { useHistory } from "react-router-dom"
import * as Yup from 'yup'
import PageWrapper from "../page-wrapper"
import Spacer from "../spacer"
import useAxios from 'axios-hooks'
import ExpensesPage from "../expenses-page"



interface FormikData {
    amountInCents: number
    currencyCode: string
    date: Date
    category: string
    RecipientEmail: string
}

const validationSchema = Yup.object({
    RecipientEmail: Yup.string().email("invalid email").required("field-requiered"),
    amountInCents: Yup.number().min(1),
    currencyCode: Yup.string().required(),
    category: Yup.string().required(),
    date: Yup.date().required(),
})


const Render = () => {

    const history = useHistory()

    var  [, execute] = useAxios({
        url: 'https://localhost:44335/create-expense',
        method: 'post'
    }, { manual: true })

    return (
        <PageWrapper maxWidth='md'>
            <Grid item xs={12}>
                <Spacer />
                <Typography variant='h6' children='Submit Expense' color='textSecondary'/>
                <Spacer />
            </Grid>
            <Formik<FormikData>
                initialValues={{} as FormikData}
                validationSchema={validationSchema}
                validateOnBlur
                validateOnChange
                validateOnMount
                onSubmit={async (values, {setFieldError, setSubmitting}) => {

                    console.log("values", values)

                    await execute({ data: values })
                        .then(function (response){
                            history.push("")
                        })
                        .catch(function (error) {

                            console.log(error.response.status);
                            console.log(error.response.data);

                            if(error.response.status === 400){

                                for (const [key, value] of Object.entries(error.response.data.errors)) {

                                    let errorMessage : string = (value as any)[0]

                                    setFieldError(key, errorMessage)
                                }
                                  
                                setSubmitting(false)
                                return
                            }
                          })
                  }}
            >
                {({ values, errors, touched, isValid, handleBlur, handleChange }) => (
                <Form>
                    <Grid container justify='center'>
                        <Grid alignItems='center'>
                            <Grid xs={12} item>
                                <TextField
                                    id="RecipientEmail"
                                    name="RecipientEmail"
                                    label="Recipient Email"
                                    type="string"
                                    value={values.RecipientEmail}
                                    onChange={handleChange}
                                    onBlur={handleBlur}
                                    error={touched.RecipientEmail && Boolean(errors.RecipientEmail)}
                                    helperText={touched.RecipientEmail && errors.RecipientEmail}
                                />
                            </Grid>
                            <Grid xs={12} item>
                                <TextField
                                    id="amountInCents"
                                    name="amountInCents"
                                    label="Amount"
                                    type="number"
                                    value={values.amountInCents}
                                    onChange={handleChange}
                                    onBlur={handleBlur}
                                    error={touched.amountInCents && Boolean(errors.amountInCents)}
                                    helperText={touched.amountInCents && errors.amountInCents}
                                />
                            </Grid>
                            <Grid xs={12} item>
                                <TextField 
                                    id="currencyCode"
                                    name="currencyCode"
                                    label="Currency"
                                    type="string"
                                    value={values.currencyCode}
                                    onChange={handleChange}
                                    onBlur={handleBlur}
                                    error={touched.currencyCode && Boolean(errors.currencyCode)}
                                    select
                                    fullWidth>
                                    <MenuItem value="USD">USD</MenuItem>
                                    <MenuItem value="CHF">CHF</MenuItem>
                                </TextField>
                            </Grid>
                            <Grid xs={12} item>
                                <TextField
                                    id="date"
                                    name="date"
                                    label='Date'
                                    type='date'
                                    value={values.date}
                                    onChange={handleChange}
                                    onBlur={handleBlur}
                                    error={touched.date && Boolean(errors.date)}
                                    helperText={touched.date && errors.date}
                                    fullWidth
                                />
                            </Grid>
                            <Grid xs={12} item>
                                <TextField                            
                                    id="category"
                                    name="category"
                                    label='Category'
                                    type='string'
                                    value={values.category}
                                    onChange={handleChange}
                                    onBlur={handleBlur}
                                    error={touched.category && Boolean(errors.category)}
                                    helperText={touched.category && errors.category}
                                />
                            </Grid>
                            <Spacer></Spacer>
                            <Grid xs={12} item>
                                <Button color="primary" variant="contained" type="submit" disabled={!isValid}>
                                    Submit
                                </Button>
                            </Grid>
                        </Grid>
                    </Grid>
                </Form>
            )}
            </Formik>
        </PageWrapper>
    )
}

export default Render