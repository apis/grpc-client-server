import React, { Component } from 'react';
import { StartRequest } from "./ipc_definitions/AcquisitionManagerService_pb";
import { AcquisitionStateReply } from "./ipc_definitions/AcquisitionManagerService_pb";
import './App.css';

class StartButton extends Component {
    constructor(props) {
        super(props);

        this.onStartButtonClick = this.onStartButtonClick.bind(this);
    }

    onStartButtonClick(e) {
        e.preventDefault();

        var startRequest = new StartRequest();
        startRequest.setSamplename(this.props.sampleNameInput);

        var client = this.props.client;

        client.start(startRequest, {}, (err, response) => {
            if (err !== null) {
                console.log(err);
                return;
            }

            this.props.onStatusChange("Last Start Action: " + (response.getResult() ? "Accepted" : "Rejected"))
        });

    }

    render() {
        return (
            <button className="button" onClick={this.onStartButtonClick} disabled={this.props.acquisitionState !== AcquisitionStateReply.AcquisitionStateEnum.IDLE}>Start</button>
        );
    }
}

export { StartButton };