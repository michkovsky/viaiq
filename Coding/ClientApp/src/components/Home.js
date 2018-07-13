import React, { Component } from 'react';

export class Home extends Component {
  displayName = Home.name

  render() {
    return (
      <div>
        <h1>Hello, world!</h1>
        <a href="./fetchdata">to react ui</a><br />
        <a href="./swagger">to swagger</a>
      </div>
    );
  }
}
