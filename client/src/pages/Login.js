import { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";

export default function Login() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  async function loginHandler(e) {
    e.preventDefault();

    try {
      const response = await fetch(`${process.env.REACT_APP_SERVER_URL}/login`, {
        method: "POST",
        headers: { "Content-type": "application/json" },
        credentials: "include",
        body: JSON.stringify({ username, password }),
      });

      if (response.status === 200) {
        const expiryTime = new Date();
        expiryTime.setTime(expiryTime.getTime() + (1 * 60 * 60 * 1000));

        document.cookie =  `token=${username}; Secure, SameSite=Strict; Expires=${expiryTime.toUTCString()}; Path=/`;
        console.log(document.cookie);

        navigate("/");
      }
    } catch (err) {
      console.log(err);
    }
  }

  return (
    <div className="login-container">
      <form className="login-form" onSubmit={loginHandler}>
        <h2>Login</h2>
        <div className="input-group">
          <label>Username</label>
          <input type="text" placeholder="username" value={username} onChange={(e) => {setUsername(e.target.value)}}/>
        </div>
        <div className="input-group">
          <label>Password</label>
          <input type="password" placeholder="password" value={password} onChange={(e) => {setPassword(e.target.value)}}/>
        </div>
        <button className="login-btn">
          Login
        </button>
      </form>
      <span>
        Don't have an account?<Link to={"/register"}>Register here</Link>
      </span>
    </div>
  );
}
