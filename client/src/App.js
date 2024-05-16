import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Layout from './components/Layout';
import Login from './pages/Login';
import Register from './pages/Register';
import './App.css';

function App() {
  return (
   <BrowserRouter>
    <Routes>
      <Route path={"/"} element={<Layout />}></Route>
      <Route path={"/login"} element={<Login />}></Route>
      <Route path={"/register"} element={<Register />}></Route>
    </Routes>
   </BrowserRouter>
  );
}

export default App;
