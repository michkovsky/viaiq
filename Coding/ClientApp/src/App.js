import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { BrowserRouter } from 'react-router-dom';

export default class App extends Component {
  displayName = App.name

  render() {
    return (
      <BrowserRouter>
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/fetchdata' component={FetchData} />
      </Layout>
      </BrowserRouter>
    );
  }
}
