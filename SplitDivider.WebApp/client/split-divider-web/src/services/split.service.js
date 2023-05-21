import { HttpStatusCode } from "axios";
import { requester } from "./request";

export const PAGE_SIZE = 20;

const getSplits = async (page, state = null) => {
  let params = {
    pageSize: PAGE_SIZE
  };

  if (state) {
    params = {
      ...params,
      state: state
    };
  }

  return requester
    .get(`/splits`, {
      withCredentials: true,
      params: params
    })
    .then(resp => {
      if (resp.status === HttpStatusCode.NotFound) {
        throw new Error("Splits not found");
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

export const patchStatusName = {
  close: "close",
  activate: "activate",
  suspend: "suspend"
};

const patchStatus = async (id, statusName) => {
  return requester
    .patch(`/split/${id}/${statusName}`, { withCredentials: true })
    .then(resp => {
      if (resp.status !== HttpStatusCode.Ok) {
        throw new Error(`Could not ${statusName} split`);
      }

      return resp.data;
    })
    .catch(err => {
      if (err.message.includes(HttpStatusCode.Unauthorized)) {
        throw new Error(`You have to authorize to ${statusName} split`);
      }

      if (err.message.includes(HttpStatusCode.Forbidden)) {
        throw new Error(`You don't have permissions to ${statusName} split`);
      }

      throw new Error("Something went wrong: " + err.message);
    });
};

const splitService = {
  getSplits,
  patchStatus
};

export default splitService;
