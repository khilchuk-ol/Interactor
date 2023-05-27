export const prepareDatetime = str => {
  let parts = str.split(" ");

  const time = parts[1].substring(0, 5);

  parts = parts[0].split("/");

  return `${parts[2]}-${parts[0]}-${parts[1]}T${time}`;
};

export const prepareDatetimeForDisplay = str => {
  let parts = str.split(" ");

  const time = parts[1].substring(0, 5);

  parts = parts[0].split("/");

  return `${parts[1]}/${parts[0]}/${parts[2]} ${time}`;
};
