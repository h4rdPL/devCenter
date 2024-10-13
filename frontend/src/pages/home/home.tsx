import { useAuth } from "../../context/authContext";

const Home = () => {
  const { user } = useAuth();
  return (
    <>
      <div>Home - {user?.company?.name}</div>
    </>
  );
};

export default Home;
