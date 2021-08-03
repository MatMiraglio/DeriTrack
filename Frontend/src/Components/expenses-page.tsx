import { Button, Grid, List, ListItem, ListItemSecondaryAction, ListItemText, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TablePagination, TableRow } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import React from 'react';
import PageWrapper from './page-wrapper';
import { Link as RouterLink } from 'react-router-dom'
import ExpenseCreatePage from './create-expense-page';
import { useFetchGet } from '../Hooks/fetch';


    interface Column {
        id: 'recipientEmail' | 'currency' | 'amount' | 'category' | 'date';
        label: string;
        minWidth?: number;
        format?: (value: any) => string;
    }
  
    const columns: Column[] = [
        { id: 'recipientEmail', label: 'User'},
        {
        id: 'currency',
        label: 'Currency',
        format: (value: number) => value.toLocaleString('en-US'),
        },
        {
            id: 'amount',
            label: 'Amount',
            format: (value: number) => {
                let dollars = value / 100;
                return dollars.toLocaleString("en-US", {style:"currency", currency:"USD"});
            },
        },
        { 
            id: 'category', 
            label: 'Category',
        },
        { 
            id: 'date', 
            label: 'Date',
            //format: (value: Date) => value.toDateString(),
        },
    ];
  
  interface Data {
    recipientEmail: string;
    currency: string;
    amount: number;
    category: string;
    date: Date;
  }
  
  function createData(recipientEmail: string, currency: string, amount: number, category : string, date: Date): Data {
    return { recipientEmail, currency, amount, category, date };
  }
  
  const rows = [
    createData('matias.miraglio@vontobel.ch', 'USD', 100, "Food", new Date()),
    createData('China', 'CHF', 200, "Food", new Date()),
    createData('Italy', 'USD', 150, "Food", new Date()),
    createData('United CHF', 'USD', 32, "Food", new Date()),
    createData('Canada', 'CHF', 37, "Food", new Date()),
    createData('Australia', 'CHF', 400, "Food", new Date()),
    createData('Germany', 'CHF', 200, "Food", new Date()),
    createData('Ireland', 'IE', 4857000, "Food", new Date()),
    createData('Mexico', 'MX', 126577691, "Food", new Date()),
    createData('Japan', 'JP', 126317000, "Food", new Date()),
    createData('France', 'FR', 67022000, "Food", new Date()),
    createData('United Kingdom', 'GB', 67545757, "Food", new Date()),
    createData('Russia', 'RU', 146793744, "Food", new Date()),
    createData('Nigeria', 'NG', 200962417, "Food", new Date()),
    createData('Brazil', 'BR', 210147125, "Food", new Date()),
  ];
  
  const useStyles = makeStyles({
    root: {
      width: '100%',
    },
    container: {
      maxHeight: 440,
    },
  });

function ExpensesPage() {

    const classes = useStyles();
    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(10);

    const { data: expenses, status } = useFetchGet<Data[]>('https://localhost:44335/all-expenses')

    const handleChangePage = (event: unknown, newPage: number) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRowsPerPage(+event.target.value);
        setPage(0);
    };

  return (
    <PageWrapper maxWidth='xl'>
      <Paper className={classes.root}>
      <Grid item xs={12} component={List}>
        <ListItem>
            <ListItemText
                primary='Expenses'
                secondary={'Here\'s a list of all expenses submited, learn more about it here: LINK'}
                primaryTypographyProps={{ variant: 'h5', gutterBottom: true }}
            />
            <ListItemSecondaryAction>
                <Button
                    color='secondary'
                    component={RouterLink}
                    to={ExpenseCreatePage.path()}
                    children='Submit New Expense'
                    variant='contained'
                />
            </ListItemSecondaryAction>
        </ListItem>
        </Grid>
        <TableContainer className={classes.container}>
          <Table stickyHeader aria-label="sticky table">
            <TableHead>
              <TableRow>
                {columns.map((column) => (
                  <TableCell
                    key={column.id}
                    style={{ minWidth: column.minWidth }}
                  >
                    {column.label}
                  </TableCell>
                ))}
              </TableRow>
            </TableHead>
            <TableBody>
              {expenses?.map((row) => {
                return (
                  <TableRow hover role="checkbox" tabIndex={-1} key={row.currency}>
                    {columns.map((column) => {
                      const value = row[column.id];
                      return (
                        <TableCell key={column.id}>
                          {column.format ? column.format(value) : value.toString()}
                        </TableCell>
                      );
                    })}
                  </TableRow>
                );
              })}
            </TableBody>
          </Table>
        </TableContainer>
        {/* <TablePagination
          rowsPerPageOptions={[10, 25, 100]}
          component="div"
          count={rows.length}
          rowsPerPage={rowsPerPage}
          page={page}
          onPageChange={handleChangePage}
          onRowsPerPageChange={handleChangeRowsPerPage}
        /> */}
      </Paper>
    </PageWrapper>
  );
}

export default ExpensesPage;