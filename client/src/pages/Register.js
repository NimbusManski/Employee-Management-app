import { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";

export default function Register() {
    const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

    async function registerHandler(e) {
        e.preventDefault();
    
        try {
          const response = await fetch(`${process.env.REACT_APP_SERVER_URL}/register`, {
            method: "POST",
            headers: { "Content-type": "application/json" },
            credentials: "include",
            body: JSON.stringify({ username, password }),
          });
    
          if (response.status === 200) {
            alert("Registration successful! You can now login");
            navigate("/login");
          }
        } catch (err) {
          console.log(err);
        }
      }

  return (
    <div className="login-container">
    <form className="login-form" onSubmit={registerHandler}>
      <h2>Register</h2>
      <div className="input-group">
        <label htmlFor="username">Username</label>
        <input type="text" id="username" name="username" onChange={(e) => {setUsername(e.target.value)}} />
      </div>
      <div className="input-group">
        <label htmlFor="password">Password</label>
        <input type="password" id="password" name="password" onChange={(e) => {setPassword(e.target.value)}} />
      </div>
      <button className='login-btn' type="submit">Register</button>
    </form>
<span>Already have an account?<Link to={'/login'}>Sign in here</Link></span>
  </div>
  )
}