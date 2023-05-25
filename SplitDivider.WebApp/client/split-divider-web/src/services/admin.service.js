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

const addRoleForUser = async (userId, roleId) => {
  return requester
    .put(`/admin/user/${userId}/role/${roleId}`, {
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

const deleteRoleForUser = async (userId, roleId) => {
  return requester
    .delete(`/admin/user/${userId}/role/${roleId}`, {
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
  addRoleForUser,
  deleteRoleForUser
};

export default adminService;
