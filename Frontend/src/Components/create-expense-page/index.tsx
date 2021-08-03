import { lazy } from 'react'

const ExpenseCreatePage = {
    path() {
        return `/create-expense/`
    },
    LazyRender: lazy(() => import('./create-expense-page')),
}

export default ExpenseCreatePage