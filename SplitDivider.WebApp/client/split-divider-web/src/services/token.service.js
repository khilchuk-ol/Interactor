const getToken = () => {
  return localStorage.getItem("token");
};

const clearToken = () => {
  localStorage.removeItem("token");
};

const saveToken = data => {
  localStorage.setItem("token", data);
};

export default {
  getToken,
  saveToken,
  clearToken
};
