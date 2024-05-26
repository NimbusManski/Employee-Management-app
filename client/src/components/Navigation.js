
import { useState, useEffect } from "react"
import { Link } from "react-router-dom"

export default function Navigation() {
const [username, setUsername] = useState("");


useEffect(() => {
  async function fetchProfile() {
  const response = await fetch(`${process.env.REACT_APP_SERVER_URL}/profile`, {
    method: "GET",
    credentials: "include"
  })
  console.log(response);
}

  

  fetchProfile();
}, [])


  return (
    <div className="navigation">
     <h1 className="nav-logo">Employee Manager</h1>
        <div className="nav-tabs">
        <h3 >Logged in as {username} |</h3>
            <Link to={"/add-employee"}>My employees</Link>
            {/* <Link to={"/-employee"}></Link>
            <Link to={"/add-employee"}></Link> */}
        </div>

    </div>
  )
}
