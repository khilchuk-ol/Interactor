const prepareGraphML = str => {
  const edgeSelectorWithId = `<edge id="`;
  const edgeSelectorWithModifiedId = `<edge id="e_`;

  return str.replaceAll(edgeSelectorWithId, edgeSelectorWithModifiedId);
};

export default {
  prepareGraphML
};
