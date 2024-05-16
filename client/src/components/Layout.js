import { Outlet } from "react-router-dom";
import Hero from "./Hero";
import Navigation from "./Navigation";

export default function Layout() {
  return (
    <main>
      <Navigation />
      <Hero />
      <Outlet />
    </main>
  );
}
