import React, { useState } from "react";
import EmailSearchBar from "../utils/inputs/EmailSearchBar";
import { required, validEmail } from "../utils/ValidationFeedback";
import { Alert } from "reactstrap";
import AdminService from "../../services/admin.service";
import UserAndRoles from "./UserAndRoles";
import trim from "validator/es/lib/trim";

export default function AdminPage() {
  const [state, setState] = useState({
    isLoading: false,
    isInitial: true,
    results: {
      user: {},
      roles: []
    },
    message: null,
    showMessage: false
  });

  const doSearch = email => {
    email = trim(email);
    setState(prev => ({
      ...prev,
      isInitial: true,
      isLoading: true
    }));

    AdminService.getUserByEmail(email)
      .then(res => {
        setState(prev => ({
          isInitial: false,
          message: null,
          showMessage: false,
          isLoading: false,
          results: res
        }));
      })
      .catch(err => {
        setState(prev => ({
          isInitial: true,
          message: err.message,
          showMessage: true,
          isLoading: false,
          results: {
            user: {},
            roles: []
          }
        }));
      });
  };

  const closeAlert = () => {
    setState(prev => ({
      ...prev,
      showMessage: false
    }));
  };

  const onChangeInput = () => {
    setState(prev => ({
      ...prev,
      isInitial: true
    }));
  };

  const setUserRoles = roles => {
    setState(prev => ({
      ...prev,
      results: {
        user: prev.results.user,
        roles: roles
      }
    }));
  };

  const onError = msg => {
    setState(prev => ({
      ...prev,
      showMessage: true,
      msg: msg
    }));
  };

  return (
    <>
      <div>
        <EmailSearchBar
          validations={[required, validEmail]}
          onSubmit={doSearch}
          onChange={onChangeInput}
        />
      </div>

      <div>
        {state.isLoading && (
          <span className="spinner-border spinner-border-sm"></span>
        )}
        {state.showMessage && (
          <Alert color="danger" isOpen={state.showMessage} toggle={closeAlert}>
            {state.message}
          </Alert>
        )}
      </div>
      {state.results && state.results.user && state.results.user.email && (
        <UserAndRoles
          user={state.results.user}
          userRoles={state.results.roles}
          setUserRoles={setUserRoles}
          onError={onError}
        />
      )}
    </>
  );
}
