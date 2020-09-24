import React, { useState, memo } from 'react';
import logo from '../../images/logo.svg';
import './App.css';
import { hot } from 'react-hot-loader';

function App() {
	const [list, setList] = useState([
		{id: 1, value: 'id1'},
		{id: 2, value: 'id2'}
	]);

	const addToList = () => {
		console.log("adding item");
		let newId = list[list.length - 1].id + 1;
		let newItem = { id: newId, value: "id" + newId };
		list.push(newItem);
		setList(list.slice());
	}

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.tsx</code> and  .
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
	<MyList list={list} />
	<MyList list={list} />
	<button onClick={addToList}>Add List Here</button>
    </div>
  );
}

const MyList = ({ list }: {list: Array<{id: number; value: string}>}) => {
	console.log('rendering: MyList')
	return (
		<>
			{list.map(item => {
				return <ListItem key={item.id} value={item.value} />
			})}
		</>
	);
}

const ListItem = memo(({ value }: {value: string}) => {
	console.log('rendering: ListItem')
	return (
		<li>{value}</li>
	)
})

export default hot(module)(App);
