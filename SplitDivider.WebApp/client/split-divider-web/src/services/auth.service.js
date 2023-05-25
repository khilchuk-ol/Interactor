import { HttpStatusCode } from "axios";
import { requester } from "./request";

const register = async (email, password, confirmPassword) => {
  return requester
    .post(
      "/auth/register",
      {
        email: email,
        password: password,
        passwordConfirmation: confirmPassword
      },
      {
        withCredentials: true
      }
    )
    .then(resp => resp.data)
    .catch(err => {
      throw new Error("Something went wrong: " + err.message);
    });
};

const login = async (email, password) => {
  return requester
    .post("/auth/login", {
      email: email,
      password: password,
      rememberMe: true
    })
    .then(resp => {
      if (resp.status === HttpStatusCode.Unauthorized) {
        throw new Error("Could not authorize user: 401");
      }

      if (resp.status === HttpStatusCode.BadRequest) {
        throw new Error("Invalid credentials: 400");
      }

      return resp.data;
    })
    .catch(err => {
      throw new Error("Something went wrong: " + err.message);
    });
};

const logout = async () => {
  return requester
    .post("/auth/signout", { withCredentials: true })
    .then(resp => resp.data)
    .catch(err => {
      throw new Error("Something went wrong: " + err.message);
    });
};

const getCurrentUser = async () => {
  return requester
    .get("/auth/me", { withCredentials: true })
    .then(resp => {
      if (resp.status === HttpStatusCode.Unauthorized) return null;

      return resp.data;
    })
    .catch(err => {
      if (err.message.includes(HttpStatusCode.Unauthorized)) {
        throw new Error("You have to authorize to access this information");
      }

      throw new Error("Something went wrong: " + err.message);
    });
};

const authServices = {
  register,
  login,
  logout,
  getCurrentUser
};

export default authServices;
