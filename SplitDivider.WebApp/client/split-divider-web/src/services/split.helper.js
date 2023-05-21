const SplitStatusCreated = 0;
const SplitStatusActivated = 1;
const SplitStatusReadyToTest = 2;
const SplitStatusSuspended = 3;
const SplitStatusClosed = 4;

const StateNames = {
  0: "Created",
  1: "Activated",
  2: "Ready to test",
  3: "Suspended",
  4: "Closed"
};

const getStateName = state => {
  return StateNames[state];
};

const canBeEdited = state => {
  return state === SplitStatusCreated || state === SplitStatusSuspended;
};

const canBeActivated = state => {
  return state === SplitStatusCreated || state === SplitStatusSuspended;
};

const canBeSuspended = state => {
  return state === SplitStatusActivated || state === SplitStatusReadyToTest;
};

const canBeClosed = state => {
  return (
    state === SplitStatusCreated ||
    state === SplitStatusReadyToTest ||
    state === SplitStatusSuspended
  );
};

const helper = {
  getStateName,
  canBeEdited,
  canBeActivated,
  canBeSuspended,
  canBeClosed
};

export default helper;
