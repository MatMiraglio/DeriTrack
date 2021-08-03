import React, { Suspense } from 'react';
import { Route, Router, Switch } from 'react-router-dom';
import './App.css';
import { createHashHistory } from 'history'
import ExpensesPage from './Components/expenses-page';
import ExpenseCreatePage from './Components/create-expense-page';

function App() {

  const history = createHashHistory()

  return (
    <div className="App">
      <Router history={history}>
        <Suspense fallback={<div />}>
          <Switch>
            <Route
              path={ExpenseCreatePage.path()}
              component={ExpenseCreatePage.LazyRender}
            />
            <Route
                path={""}
                component={ExpensesPage}
              />
          </Switch>
        </Suspense>
      </Router>
    </div>
  );
}

export default App;
