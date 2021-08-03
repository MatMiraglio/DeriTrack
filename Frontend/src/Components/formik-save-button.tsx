import { Button, CircularProgress, Fab, useTheme } from '@material-ui/core'
import { FormikContextType, useFormikContext } from 'formik'
import React, { CSSProperties } from 'react'
import { Prompt, useLocation } from 'react-router-dom'

interface Props<T = any> {
    disableOnError?: boolean
    makeLabel?: (formik: FormikContextType<T>) => string
    marginTop?: number
    style?: CSSProperties
    promptWhen?: 'NEVER' | 'LOCATION_CHANGE' | 'PATHNAME_CHANGE'
}

export default function FormikSubmitButton<T = any>({ disableOnError, makeLabel, marginTop, style = {}, promptWhen = 'LOCATION_CHANGE' }: Props<T>) {
    const formikData = useFormikContext<T>()
    const { dirty, submitForm, isSubmitting, values, errors } = formikData

    const hasError = Object.keys(errors).length > 0

    const theme = useTheme()
    const gap =theme.spacing(3)
    const zIndex = theme.zIndex.speedDial

    const generatedLabel = makeLabel?.(formikData)

    const currentLocation = useLocation()

    return <>
    <Prompt
        message={(newLocation) => {
            if(
                (dirty || isSubmitting) &&
                (
                    promptWhen === 'LOCATION_CHANGE' ||
                    (promptWhen === 'PATHNAME_CHANGE' && newLocation.pathname !== currentLocation.pathname)
                )
            ) {
                return 'It looks like you have been editing something. If you leave before saving, your changes will be lost.'
            }
            return true
        }}
    />
    <Fab
        color='secondary'
        size='large'
        variant='extended'
        disabled={isSubmitting || !dirty || (disableOnError && hasError)}
        onClick={(ev) => {
            submitForm()
        }}
        style={{
            position: 'sticky',
            bottom: gap,
            marginTop: marginTop ?? gap,
            zIndex,
            ...style
        }}
    >
        { generatedLabel ?? (isSubmitting ? 'Saving' : 'Save') }&nbsp;
        {isSubmitting ? <CircularProgress color='inherit' size={20}/> : <Button />}
    </Fab>
    </>
}