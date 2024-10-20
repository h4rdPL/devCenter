import React from "react";
import { useAuth } from "src/context/authContext";

const Home: React.FC = () => {
  const { user } = useAuth();

  return (
    <>
      <div>Home - {user?.company?.name}</div>
    </>
  );
};

export default Home;
