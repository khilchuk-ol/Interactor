import axios from "axios";
import tokenService from "./token.service";
const API_URL = "http://localhost:5001/api";

export const requester = axios.create({
  baseURL: API_URL,
  headers: {
    common: {
      "x-auth-token": tokenService.getToken()
    }
  }
});
