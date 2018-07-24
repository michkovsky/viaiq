import React, { Component } from 'react';
import { Label, InputGroup } from 'react-bootstrap';

export class FetchData extends Component {
  displayName = FetchData.name

  constructor(props) {
    super(props);
    this.state = { tweets: [], loading: false, startDate: "2017-01-01", endDate: "2018-01-01", };
    this.fetchTwitterData = this.fetchTwitterData.bind(this);
    this.dateChange=this.dateChange.bind(this);

  }
  fetchTwitterData() {
    this.state.loading = true;
    this.setState(this.state);
    var url=["api/TwitterData/GetTweets?startDate=",this.state.startDate,"&endDate=",this.state.endDate].join("");
    fetch(url)
      .then(response => response.json())
      .then(data => {
        this.state.tweets=data;
        this.state.loading=false;
        this.setState(this.state);
      });
  }
  dateChange(evt){
    //it's too late to introduce redux
    var element = evt.target;
    var name=element.name;
    var val = element.value;
    this.state[name]=val;
    this.setState(this.state);
  }

  static renderTweetsTable(tweets) {
    return (
      <table className='table'>
        <thead>
          <tr>
            <th>id</th>
            <th>stamp</th>
            <th>text</th>
          </tr>
        </thead>
        <tbody>
          {tweets.map(tweets =>
            <tr key={tweets.id}>
              <td>{tweets.id}</td>
              <td>{tweets.stamp}</td>
              <td>{tweets.text}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderTweetsTable(this.state.tweets);

    return (
      <div>
        <h1>Render tweets</h1>
        <p>This component demonstrates fetching data from the server.</p>
        <InputGroup><Label>Input date range</Label>
          <br />
          <Label>Start Date</Label><input type="date" name="startDate"  value={this.state.startDate} onChange={this.dateChange} /><br />
          <Label>End Date</Label><input type="date" name="endDate"  value={this.state.endDate} onChange={this.dateChange}  /><br />
          <button onClick={this.fetchTwitterData}>fetch data</button>
        </InputGroup>
        {contents}
      </div>
    );
  }
}
