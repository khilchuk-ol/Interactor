import { HttpStatusCode } from "axios";
import { requester } from "./request";

const register = async (email, password, confirmPassword) => {
  return requester
    .post(
      "/auth/signup",
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

      return resp;
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
    .then(resp => resp.data)
    .catch(err => {
      throw new Error("Something went wrong: " + err.message);
    });
};

const getCachedUser = async () => {
  return new Promise((resolve, reject) => {
    const user = JSON.parse(localStorage.getItem("user"));
    if (user) {
      resolve(user);
    } else {
      reject();
    }
  });
};

const clearCache = () => {
  localStorage.removeItem("user");
};

const saveToCache = data => {
  localStorage.setItem("user", JSON.stringify(data));
};

const authServices = {
  register,
  login,
  logout,
  getCurrentUser,
  getCachedUser,
  clearCache,
  saveToCache
};

export default authServices;
