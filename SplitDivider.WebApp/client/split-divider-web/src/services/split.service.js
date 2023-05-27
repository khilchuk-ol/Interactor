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

const getSplit = async id => {
  return requester
    .get(`/splits/${id}`, {
      withCredentials: true
    })
    .then(resp => {
      if (resp.status === HttpStatusCode.NotFound) {
        throw new Error("Split not found");
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

const getSplitGraph = async id => {
  return requester
    .get(`/splits/${id}/graph`, {
      withCredentials: true,
      headers: {
        "Content-Type": "application/octet-stream"
      }
    })
    .then(resp => {
      if (resp.status === HttpStatusCode.NotFound) {
        throw new Error("Split not found");
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

const getSplitUsers = async id => {
  return requester
    .get(`/splits/${id}/users`, {
      withCredentials: true
    })
    .then(resp => {
      if (resp.status === HttpStatusCode.NotFound) {
        throw new Error("Information not found");
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
    .patch(`/splits/${id}/${statusName}`, { withCredentials: true })
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

const postSplit = async (
  name,
  gender,
  minRegDt,
  countryIds,
  actionsWeights
) => {
  let createParams = {
    name: name,
    gender: gender,
    minRegDt: minRegDt,
    countryIds: countryIds,
    actionsWeights: actionsWeights
  };

  return requester
    .post(`/splits`, createParams, {
      withCredentials: true
    })
    .then(resp => resp.data)
    .catch(err => {
      if (err.message.includes(HttpStatusCode.Unauthorized)) {
        throw new Error("You have to authorize to create split");
      }

      if (err.message.includes(HttpStatusCode.Forbidden)) {
        throw new Error("You don't have permissions to create split");
      }

      if (err.message) throw new Error("Something went wrong: " + err.message);
    });
};

const patchSplit = async (
  id,
  name,
  gender,
  minRegDt,
  countryIds,
  actionsWeights
) => {
  let params = {
    name: name,
    gender: gender,
    minRegDt: minRegDt,
    countryIds: countryIds,
    actionsWeights: actionsWeights
  };

  return requester
    .patch(`/splits/${id}`, params, {
      withCredentials: true
    })
    .then(resp => resp.data)
    .catch(err => {
      if (err.message.includes(HttpStatusCode.Unauthorized)) {
        throw new Error("You have to authorize to create split");
      }

      if (err.message.includes(HttpStatusCode.Forbidden)) {
        throw new Error("You don't have permissions to create split");
      }

      if (err.message) throw new Error("Something went wrong: " + err.message);
    });
};

const splitService = {
  getSplits,
  patchStatus,
  postSplit,
  getSplit,
  getSplitUsers,
  getSplitGraph,
  patchSplit
};

export default splitService;
