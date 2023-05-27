import { HttpStatusCode } from "axios";
import { requester } from "./request";

const getUsers = async () => {
  return requester
    .get(`/admin/users`, {
      withCredentials: true
    })
    .then(resp => {
      if (resp.status === HttpStatusCode.NotFound) {
        throw new Error("Users not found");
      }

      return resp.data;
    })
    .catch(err => {
      if (err.message.includes(HttpStatusCode.Unauthorized)) {
        throw new Error("You have to authorize to access this information");
      }

      if (err.message.includes(HttpStatusCode.Forbidden)) {
        throw new Error("You don't have access to this information");
      }

      if (err.message) throw new Error("Something went wrong: " + err.message);
    });
};

const getUserByEmail = email => {
  return requester
    .get(`/admin/users/${email}`, {
      withCredentials: true
    })
    .then(resp => {
      if (resp.status === HttpStatusCode.NotFound) {
        throw new Error("User not found");
      }

      return resp.data;
    })
    .catch(err => {
      if (err.message.includes(HttpStatusCode.Unauthorized)) {
        throw new Error("You have to authorize to access this information");
      }

      if (err.message.includes(HttpStatusCode.Forbidden)) {
        throw new Error("You don't have access to this information");
      }

      if (err.message.includes(HttpStatusCode.NotFound)) {
        throw new Error("User with such email was not found");
      }

      if (err.message) throw new Error("Something went wrong: " + err.message);
    });
};

const fetchRoles = async () => {
  return requester
    .get(`/admin/roles`, {
      withCredentials: true
    })
    .then(resp => {
      if (resp.status === HttpStatusCode.NotFound) {
        throw new Error("Roles not found");
      }

      return resp.data;
    })
    .catch(err => {
      if (err.message.includes(HttpStatusCode.Unauthorized)) {
        throw new Error("You have to authorize to access this information");
      }

      if (err.message.includes(HttpStatusCode.Forbidden)) {
        throw new Error("You don't have access to this information");
      }

      if (err.message) throw new Error("Something went wrong: " + err.message);
    });
};

const addRoleForUser = async (userId, role) => {
  return requester
    .put(`/admin/user/${userId}/role/${role}`, {
      withCredentials: true
    })
    .then(resp => {
      if (resp.status === HttpStatusCode.NotFound) {
        throw new Error("User or role is not found");
      }

      return resp.data;
    })
    .catch(err => {
      if (err.message.includes(HttpStatusCode.Unauthorized)) {
        throw new Error("You have to authorize to assign roles to users");
      }

      if (err.message.includes(HttpStatusCode.Forbidden)) {
        throw new Error("You don't have permissions to assign roles to users");
      }

      if (err.message) throw new Error("Something went wrong: " + err.message);
    });
};

const deleteRoleForUser = async (userId, role) => {
  return requester
    .delete(`/admin/user/${userId}/role/${role}`, {
      withCredentials: true
    })
    .then(resp => {
      if (resp.status === HttpStatusCode.NotFound) {
        throw new Error("User or role is not found");
      }

      return resp.data;
    })
    .catch(err => {
      if (err.message.includes(HttpStatusCode.Unauthorized)) {
        throw new Error("You have to authorize to revoke roles from users");
      }

      if (err.message.includes(HttpStatusCode.Forbidden)) {
        throw new Error(
          "You don't have permissions to revoke roles from users"
        );
      }

      if (err.message) throw new Error("Something went wrong: " + err.message);
    });
};

const adminService = {
  getUsers,
  getUserByEmail,
  addRoleForUser,
  deleteRoleForUser,
  fetchRoles
};

export default adminService;
