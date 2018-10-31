import React, { Component } from 'react';
import { AcquisitionStateLabel, AcquisitionCompletionStateLabel, CurrentSampleNameLabel, StatusLabel } from './Labels';
import { StartButton } from './StartButton';
import { StopButton } from './StopButton';
import { SampleNameInput } from './SampleNameInput';
import { AcquisitionManagerServiceClient } from "./ipc_definitions/AcquisitionManagerService_grpc_web_pb";
import { Empty } from "google-protobuf/google/protobuf/empty_pb";
import './App.css';

class App extends Component {
  constructor(props) {
    super(props);

    this.onAcquisitionStateStreamData = this.onAcquisitionStateStreamData.bind(this);
    this.reconnectAcquisitionStateStream = this.reconnectAcquisitionStateStream.bind(this);
    this.onAcquisitionCompletionStateStreamData = this.onAcquisitionCompletionStateStreamData.bind(this);
    this.reconnectAcquisitionCompletionStateStream = this.reconnectAcquisitionCompletionStateStream.bind(this);
    this.onCurrentSampleNameStreamData = this.onCurrentSampleNameStreamData.bind(this);
    this.reconnectCurrentSampleNameStream = this.reconnectCurrentSampleNameStream.bind(this);
    this.onStatusChange = this.onStatusChange.bind(this);
    this.onVisibilityChange = this.onVisibilityChange.bind(this);
    this.onSampleNameInputChange = this.onSampleNameInputChange.bind(this);

    this.state = {
      client: undefined,
      acquisitionState: undefined,
      acquisitionCompletionState: undefined,
      currentSampleName: undefined,
      acquisitionStateStream: undefined,
      acquisitionCompletionStateStream: undefined,
      currentSampleNameStream: undefined,
      status: "",
      sampleNameInput: "JavaScript Sample 001"
    };
  }

  componentDidMount() {
    var client = new AcquisitionManagerServiceClient('http://localhost:11973');
    console.log(client);
    this.setState({ client: client }, () => {
      this.reconnectAcquisitionStateStream();
      this.reconnectAcquisitionCompletionStateStream();
      this.reconnectCurrentSampleNameStream();
      document.addEventListener("visibilitychange", this.onVisibilityChange, false);
    });
  }

  onVisibilityChange() {
    if (!document.hidden) {
      console.log("visible");

      // this.reconnectAcquisitionStateStream(); 
      window.location.reload();
    }
  }

  onAcquisitionCompletionStateStreamData(response) {
    var state = response.getAcquisitioncompletionstateenum();
    this.setState({ acquisitionCompletionState: state });
  }

  onAcquisitionStateStreamData(response) {
    var state = response.getAcquisitionstateenum();
    this.setState({ acquisitionState: state });
  }

  onCurrentSampleNameStreamData(response) {
    var state = response.getCurrentsamplename();
    this.setState({ currentSampleName: state, sampleNameInput: state });
  }

  onStatusChange(status) {
    this.setState({ status: status });
  }

  onSampleNameInputChange(sampleNameInput) {
    console.log(sampleNameInput);

    this.setState({ sampleNameInput: sampleNameInput });
  }

  reconnectAcquisitionStateStream() {
    var client = this.state.client;
    var acquisitionStateStream = this.state.acquisitionStateStream;

    console.log(acquisitionStateStream);

    // if (acquisitionStateStream !== undefined) {
    //   acquisitionStateStream.cancel();
    // }

    client.getAcquisitionState(new Empty(), {}, (err, response) => {
      if (err !== null) {
        console.log(err);
        return;
      }

      var s = response.getAcquisitionstateenum();
      console.log("AcquisitionState: " + s);

      this.setState({ acquisitionState: s });
    });

    acquisitionStateStream = client.getAcquisitionStateStream(new Empty(), {});

    acquisitionStateStream.on('data', (response) => {
      this.onAcquisitionStateStreamData(response)
    });

    acquisitionStateStream.on('status', (status) => {
      console.log(status.code);
      console.log(status.details);
      console.log(status.metadata);
    });

    // acquisitionStateStream.on('close', () => { 
    // this.reconnectAcquisitionStateStream(); 
    // });

    acquisitionStateStream.on('error', (error) => {
      if (error.message === "stream timeout") {
        this.reconnectAcquisitionStateStream();
      }

      console.log(error);
    });

    this.setState({ acquisitionStateStream: acquisitionStateStream });
  }

  reconnectAcquisitionCompletionStateStream() {
    var client = this.state.client;
    // var acquisitionCompletionStateStream = this.state.acquisitionCompletionStateStream;

    // if (acquisitionCompletionStateStream !== undefined) {
    //   acquisitionCompletionStateStream.close();
    // }

    client.getAcquisitionCompletionState(new Empty(), {}, (err, response) => {
      if (err !== null) {
        console.log(err);
        return;
      }

      var state = response.getAcquisitioncompletionstateenum();
      this.setState({ acquisitionCompletionState: state });
    });

    var acquisitionCompletionStateStream = client.getAcquisitionCompletionStateStream(new Empty(), {});

    acquisitionCompletionStateStream.on('data', (response) => {
      this.onAcquisitionCompletionStateStreamData(response)
    });

    acquisitionCompletionStateStream.on('end', () => {
      console.log("AcquisitionCompletionStateStream end!");
    });

    acquisitionCompletionStateStream.on('close', () => {
      console.log("AcquisitionCompletionStateStream close!");
      // this.reconnectAcquisitionCompletionStateStream(); 
    });

    acquisitionCompletionStateStream.on('error', (error) => {
      console.log("AcquisitionCompletionStateStream error!");
      if (error.message === "stream timeout") {
        this.reconnectAcquisitionCompletionStateStream();
      }
      console.log(error);
    });

    this.setState({ acquisitionCompletionStateStream: acquisitionCompletionStateStream });
  }

  reconnectCurrentSampleNameStream() {
    var client = this.state.client;
    var currentSampleNameStream = this.state.currentSampleNameStream;

    client.getCurrentSampleName(new Empty(), {}, (err, response) => {
      if (err !== null) {
        console.log(err);
        return;
      }

      var s = response.getCurrentsamplename();
      console.log("CurrentSampleName: " + s);

      this.setState({ currentSampleName: s, sampleNameInput: s });
    });

    currentSampleNameStream = client.getCurrentSampleNameStream(new Empty(), {});

    currentSampleNameStream.on('data', (response) => {
      this.onCurrentSampleNameStreamData(response)
    });

    currentSampleNameStream.on('status', (status) => {
      console.log(status.code);
      console.log(status.details);
      console.log(status.metadata);
    });

    // acquisitionStateStream.on('close', () => { 
    // this.reconnectAcquisitionStateStream(); 
    // });

    currentSampleNameStream.on('error', (error) => {
      if (error.message === "stream timeout") {
        this.reconnectCurrentSampleNameStream();
      }

      console.log(error);
    });

    this.setState({ currentSampleNameStream: currentSampleNameStream });
  }

  render() {
    return (
      <div>
        <SampleNameInput sampleNameInput={this.state.sampleNameInput} onSampleNameInputChange={this.onSampleNameInputChange} />
        <div className="buttons">
          <StartButton client={this.state.client} onStatusChange={this.onStatusChange} acquisitionState={this.state.acquisitionState} sampleNameInput={this.state.sampleNameInput} />
          <div className="buttonSpacer" />
          <StopButton client={this.state.client} onStatusChange={this.onStatusChange} acquisitionState={this.state.acquisitionState} />
        </div>
        <CurrentSampleNameLabel currentSampleName={this.state.currentSampleName} />
        <AcquisitionStateLabel acquisitionState={this.state.acquisitionState} />
        <AcquisitionCompletionStateLabel acquisitionCompletionState={this.state.acquisitionCompletionState} />
        <StatusLabel status={this.state.status} />
      </div>
    );
  }
}

export default App;