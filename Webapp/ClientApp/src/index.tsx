import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './components/App/App';
import { BrowserRouter, Link, Route } from 'react-router-dom';
import * as serviceWorker from './service-worker';
import * as axios from 'axios';

const baseUrl = process.env.PUBLIC_URL;
const rootElement = document.getElementById('root');
console.log(baseUrl);

ReactDOM.render(
    <BrowserRouter basename={baseUrl}>
        <Route exact path="/">
            <App />
            <Link to="/fake">Click here</Link>
        </Route>
        <Route path="/fake">
            <h1>Fake path</h1>
        </Route>
    </BrowserRouter>,
rootElement);

fetch('api/weatherforecast')
// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.register();
