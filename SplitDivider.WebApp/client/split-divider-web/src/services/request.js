import axios from "axios";

const API_URL = "http://localhost:5001/api";

export const requester = axios.create({
  baseURL: API_URL
});
