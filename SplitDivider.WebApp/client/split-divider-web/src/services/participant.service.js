import { HttpStatusCode } from "axios";
import { requester } from "./request";

const getParticipantsSplitGroups = async id => {
  return requester
    .get(`/users/${id}/splits`, {
      withCredentials: true
    })
    .then(resp => {
      if (resp.status === HttpStatusCode.NotFound) {
        throw new Error("User with such id was not found");
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

const participantServices = {
  getParticipantsSplitGroups
};

export default participantServices;
