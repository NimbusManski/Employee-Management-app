import { useState } from 'react';
import { FiPlus } from 'react-icons/fi';

export default function Hero() {
const [name, setName] = useState("");
const [birthDate, setBirthDate] = useState("");
const [salary, setSalary] = useState("");
const [startDate, setStartDate] = useState("");


async function createEmployeeHandler(e) {
    e.preventDefault();

    try {
      const response =  await fetch("http://localhost:5113/create-employee", {
            mehtod: "POST",
            headers: {"Content-type" : "application/json"},
            credentials: 'include',
            body: { name, birthDate, salary, startDate }
         })

         if(response.status === 201) {
            alert("Employee successfully created");
         }


    } catch(err) {
        console.log(err);
    }
}

  return (
    <div className='hero'>
       
        <h3>Add New Employee</h3>
        <FiPlus style={{padding: "10px"}}/>
        <form className='add-employee-form' onSubmit={createEmployeeHandler}>
            <input type='text' value={name} onChange={(e) => {setName(e.target.value)}} placeholder='Employee name'></input>
            <input type='text' value={birthDate} onChange={(e) => {setBirthDate(e.target.value)}} placeholder='Employee date-of-birth'></input>
            <input type='text' value={salary} onChange={(e) => {setSalary(e.target.value)}} placeholder='Employee salary'></input>
            <input type='text' value={startDate} onChange={(e) => {setStartDate(e.target.value)}} placeholder='Employee start date'></input>
            <button className='add-btn'>Add Employee</button>
        </form>
    </div>
  )
}
