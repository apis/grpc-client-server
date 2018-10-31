import React, { Component } from 'react';
import { AcquisitionStateReply } from "./ipc_definitions/AcquisitionManagerService_pb";
import { Empty } from "google-protobuf/google/protobuf/empty_pb";
import './App.css';

class StopButton extends Component {
    constructor(props) {
        super(props);

        this.onStopButtonClick = this.onStopButtonClick.bind(this);
    }

    onStopButtonClick(e) {
        e.preventDefault();

        var client = this.props.client;

        client.stop(new Empty(), {}, (err, response) => {
            if (err !== null) {
                console.log(err);
                return;
            }

            this.props.onStatusChange("Last Stop Action: " + (response.getResult() ? "Accepted" : "Rejected"))
        });
    }

    render() {
        return (
            <button className="button" onClick={this.onStopButtonClick} disabled={this.props.acquisitionState !== AcquisitionStateReply.AcquisitionStateEnum.RUNNING}>Stop</button>
        );
    }
}

class SampleNameInput extends Component {
    constructor(props) {
        super(props);

        this.onSampleNameInputChange = this.onSampleNameInputChange.bind(this);
    }

    onSampleNameInputChange(event) {
        event.preventDefault();

        this.props.onSampleNameInputChange(event.target.value);
    }

    render() {
        return (
            <form>
                <label>
                    Sample Name:
                    <input type="text" value={this.props.sampleNameInput} onChange={this.onSampleNameInputChange} />
                </label>
            </form>
        );
    }
}

export { StopButton, SampleNameInput };