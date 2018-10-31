import React, { Component } from 'react';
import { AcquisitionStateReply, AcquisitionCompletionStateReply } from "./ipc_definitions/AcquisitionManagerService_pb";
import './App.css';

class AcquisitionStateLabel extends Component {
    render() {
        var acquisitionStateString;

        switch (this.props.acquisitionState) {
            case AcquisitionStateReply.AcquisitionStateEnum.IDLE:
                acquisitionStateString = "Idle";
                break;
            case AcquisitionStateReply.AcquisitionStateEnum.RUNNING:
                acquisitionStateString = "Running";
                break;
            default:
                acquisitionStateString = "Unknown";
        }

        return (
            <div className="label">
                <span className="labelHeader">Acquisition State:&nbsp;</span>
                <span className="labelText">{acquisitionStateString}</span>
            </div>
        );
    }
}

class AcquisitionCompletionStateLabel extends Component {
    render() {
        var acquisitionCompletionStateString;

        switch (this.props.acquisitionCompletionState) {
            case AcquisitionCompletionStateReply.AcquisitionCompletionStateEnum.INCOMPLETE:
                acquisitionCompletionStateString = "Incomplete";
                break;
            case AcquisitionCompletionStateReply.AcquisitionCompletionStateEnum.STOPPED:
                acquisitionCompletionStateString = "Stopped";
                break;
            case AcquisitionCompletionStateReply.AcquisitionCompletionStateEnum.FAILED:
                acquisitionCompletionStateString = "Failed";
                break;
            case AcquisitionCompletionStateReply.AcquisitionCompletionStateEnum.SUCCESSFULLY_COMPLETED:
                acquisitionCompletionStateString = "Successfully Completed";
                break;
            default:
                acquisitionCompletionStateString = "Unknown";
        }

        return (
            <div className="label">
                <span className="labelHeader">Acquisition Completion State:&nbsp;</span>
                <span className="labelText">{acquisitionCompletionStateString}</span>
            </div>
        );
    }
}

class CurrentSampleNameLabel extends Component {
    render() {
        return (
            <div className="label">
                <span className="labelHeader">Current Sample Name:&nbsp;</span>
                <span className="labelText">{this.props.currentSampleName}</span>
            </div>
        );
    }
}

class StatusLabel extends Component {
    render() {
        return (
            <div className="status">
                <span className="statusText">{this.props.status}</span>
            </div>
        );
    }
}

export { AcquisitionStateLabel, AcquisitionCompletionStateLabel, CurrentSampleNameLabel, StatusLabel };
