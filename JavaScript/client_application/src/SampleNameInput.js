import React, { Component } from 'react';
import './App.css';

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

export { SampleNameInput };