/**
 * @fileoverview gRPC-Web generated client stub for ipc_definitions
 * @enhanceable
 * @public
 */

// GENERATED CODE -- DO NOT EDIT!



const grpc = {};
grpc.web = require('grpc-web');


var google_protobuf_empty_pb = require('google-protobuf/google/protobuf/empty_pb.js')
const proto = {};
proto.ipc_definitions = require('./AcquisitionManagerService_pb.js');

/**
 * @param {string} hostname
 * @param {?Object} credentials
 * @param {?Object} options
 * @constructor
 * @struct
 * @final
 */
proto.ipc_definitions.AcquisitionManagerServiceClient =
    function(hostname, credentials, options) {
  if (!options) options = {};
  options['format'] = 'text';

  /**
   * @private @const {!grpc.web.GrpcWebClientBase} The client
   */
  this.client_ = new grpc.web.GrpcWebClientBase(options);

  /**
   * @private @const {string} The hostname
   */
  this.hostname_ = hostname;

  /**
   * @private @const {?Object} The credentials to be used to connect
   *    to the server
   */
  this.credentials_ = credentials;

  /**
   * @private @const {?Object} Options for the client
   */
  this.options_ = options;
};


/**
 * @param {string} hostname
 * @param {?Object} credentials
 * @param {?Object} options
 * @constructor
 * @struct
 * @final
 */
proto.ipc_definitions.AcquisitionManagerServicePromiseClient =
    function(hostname, credentials, options) {
  if (!options) options = {};
  options['format'] = 'text';

  /**
   * @private @const {!proto.ipc_definitions.AcquisitionManagerServiceClient} The delegate callback based client
   */
  this.delegateClient_ = new proto.ipc_definitions.AcquisitionManagerServiceClient(
      hostname, credentials, options);

};


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.ipc_definitions.StartRequest,
 *   !proto.ipc_definitions.StartReply>}
 */
const methodInfo_Start = new grpc.web.AbstractClientBase.MethodInfo(
  proto.ipc_definitions.StartReply,
  /** @param {!proto.ipc_definitions.StartRequest} request */
  function(request) {
    return request.serializeBinary();
  },
  proto.ipc_definitions.StartReply.deserializeBinary
);


/**
 * @param {!proto.ipc_definitions.StartRequest} request The
 *     request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.Error, ?proto.ipc_definitions.StartReply)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.ipc_definitions.StartReply>|undefined}
 *     The XHR Node Readable Stream
 */
proto.ipc_definitions.AcquisitionManagerServiceClient.prototype.start =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/ipc_definitions.AcquisitionManagerService/Start',
      request,
      metadata,
      methodInfo_Start,
      callback);
};


/**
 * @param {!proto.ipc_definitions.StartRequest} request The
 *     request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.ipc_definitions.StartReply>}
 *     The XHR Node Readable Stream
 */
proto.ipc_definitions.AcquisitionManagerServicePromiseClient.prototype.start =
    function(request, metadata) {
  return new Promise((resolve, reject) => {
    this.delegateClient_.start(
      request, metadata, (error, response) => {
        error ? reject(error) : resolve(response);
      });
  });
};


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.ipc_definitions.StopRequest,
 *   !proto.ipc_definitions.StopReply>}
 */
const methodInfo_Stop = new grpc.web.AbstractClientBase.MethodInfo(
  proto.ipc_definitions.StopReply,
  /** @param {!proto.ipc_definitions.StopRequest} request */
  function(request) {
    return request.serializeBinary();
  },
  proto.ipc_definitions.StopReply.deserializeBinary
);


/**
 * @param {!proto.ipc_definitions.StopRequest} request The
 *     request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.Error, ?proto.ipc_definitions.StopReply)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.ipc_definitions.StopReply>|undefined}
 *     The XHR Node Readable Stream
 */
proto.ipc_definitions.AcquisitionManagerServiceClient.prototype.stop =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/ipc_definitions.AcquisitionManagerService/Stop',
      request,
      metadata,
      methodInfo_Stop,
      callback);
};


/**
 * @param {!proto.ipc_definitions.StopRequest} request The
 *     request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.ipc_definitions.StopReply>}
 *     The XHR Node Readable Stream
 */
proto.ipc_definitions.AcquisitionManagerServicePromiseClient.prototype.stop =
    function(request, metadata) {
  return new Promise((resolve, reject) => {
    this.delegateClient_.stop(
      request, metadata, (error, response) => {
        error ? reject(error) : resolve(response);
      });
  });
};


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.google.protobuf.Empty,
 *   !proto.ipc_definitions.CurrentSampleNameReply>}
 */
const methodInfo_GetCurrentSampleName = new grpc.web.AbstractClientBase.MethodInfo(
  proto.ipc_definitions.CurrentSampleNameReply,
  /** @param {!proto.google.protobuf.Empty} request */
  function(request) {
    return request.serializeBinary();
  },
  proto.ipc_definitions.CurrentSampleNameReply.deserializeBinary
);


/**
 * @param {!proto.google.protobuf.Empty} request The
 *     request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.Error, ?proto.ipc_definitions.CurrentSampleNameReply)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.ipc_definitions.CurrentSampleNameReply>|undefined}
 *     The XHR Node Readable Stream
 */
proto.ipc_definitions.AcquisitionManagerServiceClient.prototype.getCurrentSampleName =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/ipc_definitions.AcquisitionManagerService/GetCurrentSampleName',
      request,
      metadata,
      methodInfo_GetCurrentSampleName,
      callback);
};


/**
 * @param {!proto.google.protobuf.Empty} request The
 *     request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.ipc_definitions.CurrentSampleNameReply>}
 *     The XHR Node Readable Stream
 */
proto.ipc_definitions.AcquisitionManagerServicePromiseClient.prototype.getCurrentSampleName =
    function(request, metadata) {
  return new Promise((resolve, reject) => {
    this.delegateClient_.getCurrentSampleName(
      request, metadata, (error, response) => {
        error ? reject(error) : resolve(response);
      });
  });
};


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.google.protobuf.Empty,
 *   !proto.ipc_definitions.CurrentSampleNameReply>}
 */
const methodInfo_GetCurrentSampleNameStream = new grpc.web.AbstractClientBase.MethodInfo(
  proto.ipc_definitions.CurrentSampleNameReply,
  /** @param {!proto.google.protobuf.Empty} request */
  function(request) {
    return request.serializeBinary();
  },
  proto.ipc_definitions.CurrentSampleNameReply.deserializeBinary
);


/**
 * @param {!proto.google.protobuf.Empty} request The request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @return {!grpc.web.ClientReadableStream<!proto.ipc_definitions.CurrentSampleNameReply>}
 *     The XHR Node Readable Stream
 */
proto.ipc_definitions.AcquisitionManagerServiceClient.prototype.getCurrentSampleNameStream =
    function(request, metadata) {
  return this.client_.serverStreaming(this.hostname_ +
      '/ipc_definitions.AcquisitionManagerService/GetCurrentSampleNameStream',
      request,
      metadata,
      methodInfo_GetCurrentSampleNameStream);
};


/**
 * @param {!proto.google.protobuf.Empty} request The request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @return {!grpc.web.ClientReadableStream<!proto.ipc_definitions.CurrentSampleNameReply>}
 *     The XHR Node Readable Stream
 */
proto.ipc_definitions.AcquisitionManagerServicePromiseClient.prototype.getCurrentSampleNameStream =
    function(request, metadata) {
  return this.delegateClient_.client_.serverStreaming(this.delegateClient_.hostname_ +
      '/ipc_definitions.AcquisitionManagerService/GetCurrentSampleNameStream',
      request,
      metadata,
      methodInfo_GetCurrentSampleNameStream);
};


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.google.protobuf.Empty,
 *   !proto.ipc_definitions.AcquisitionStateReply>}
 */
const methodInfo_GetAcquisitionState = new grpc.web.AbstractClientBase.MethodInfo(
  proto.ipc_definitions.AcquisitionStateReply,
  /** @param {!proto.google.protobuf.Empty} request */
  function(request) {
    return request.serializeBinary();
  },
  proto.ipc_definitions.AcquisitionStateReply.deserializeBinary
);


/**
 * @param {!proto.google.protobuf.Empty} request The
 *     request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.Error, ?proto.ipc_definitions.AcquisitionStateReply)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.ipc_definitions.AcquisitionStateReply>|undefined}
 *     The XHR Node Readable Stream
 */
proto.ipc_definitions.AcquisitionManagerServiceClient.prototype.getAcquisitionState =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/ipc_definitions.AcquisitionManagerService/GetAcquisitionState',
      request,
      metadata,
      methodInfo_GetAcquisitionState,
      callback);
};


/**
 * @param {!proto.google.protobuf.Empty} request The
 *     request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.ipc_definitions.AcquisitionStateReply>}
 *     The XHR Node Readable Stream
 */
proto.ipc_definitions.AcquisitionManagerServicePromiseClient.prototype.getAcquisitionState =
    function(request, metadata) {
  return new Promise((resolve, reject) => {
    this.delegateClient_.getAcquisitionState(
      request, metadata, (error, response) => {
        error ? reject(error) : resolve(response);
      });
  });
};


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.google.protobuf.Empty,
 *   !proto.ipc_definitions.AcquisitionStateReply>}
 */
const methodInfo_GetAcquisitionStateStream = new grpc.web.AbstractClientBase.MethodInfo(
  proto.ipc_definitions.AcquisitionStateReply,
  /** @param {!proto.google.protobuf.Empty} request */
  function(request) {
    return request.serializeBinary();
  },
  proto.ipc_definitions.AcquisitionStateReply.deserializeBinary
);


/**
 * @param {!proto.google.protobuf.Empty} request The request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @return {!grpc.web.ClientReadableStream<!proto.ipc_definitions.AcquisitionStateReply>}
 *     The XHR Node Readable Stream
 */
proto.ipc_definitions.AcquisitionManagerServiceClient.prototype.getAcquisitionStateStream =
    function(request, metadata) {
  return this.client_.serverStreaming(this.hostname_ +
      '/ipc_definitions.AcquisitionManagerService/GetAcquisitionStateStream',
      request,
      metadata,
      methodInfo_GetAcquisitionStateStream);
};


/**
 * @param {!proto.google.protobuf.Empty} request The request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @return {!grpc.web.ClientReadableStream<!proto.ipc_definitions.AcquisitionStateReply>}
 *     The XHR Node Readable Stream
 */
proto.ipc_definitions.AcquisitionManagerServicePromiseClient.prototype.getAcquisitionStateStream =
    function(request, metadata) {
  return this.delegateClient_.client_.serverStreaming(this.delegateClient_.hostname_ +
      '/ipc_definitions.AcquisitionManagerService/GetAcquisitionStateStream',
      request,
      metadata,
      methodInfo_GetAcquisitionStateStream);
};


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.google.protobuf.Empty,
 *   !proto.ipc_definitions.AcquisitionCompletionStateReply>}
 */
const methodInfo_GetAcquisitionCompletionState = new grpc.web.AbstractClientBase.MethodInfo(
  proto.ipc_definitions.AcquisitionCompletionStateReply,
  /** @param {!proto.google.protobuf.Empty} request */
  function(request) {
    return request.serializeBinary();
  },
  proto.ipc_definitions.AcquisitionCompletionStateReply.deserializeBinary
);


/**
 * @param {!proto.google.protobuf.Empty} request The
 *     request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.Error, ?proto.ipc_definitions.AcquisitionCompletionStateReply)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.ipc_definitions.AcquisitionCompletionStateReply>|undefined}
 *     The XHR Node Readable Stream
 */
proto.ipc_definitions.AcquisitionManagerServiceClient.prototype.getAcquisitionCompletionState =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/ipc_definitions.AcquisitionManagerService/GetAcquisitionCompletionState',
      request,
      metadata,
      methodInfo_GetAcquisitionCompletionState,
      callback);
};


/**
 * @param {!proto.google.protobuf.Empty} request The
 *     request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.ipc_definitions.AcquisitionCompletionStateReply>}
 *     The XHR Node Readable Stream
 */
proto.ipc_definitions.AcquisitionManagerServicePromiseClient.prototype.getAcquisitionCompletionState =
    function(request, metadata) {
  return new Promise((resolve, reject) => {
    this.delegateClient_.getAcquisitionCompletionState(
      request, metadata, (error, response) => {
        error ? reject(error) : resolve(response);
      });
  });
};


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.google.protobuf.Empty,
 *   !proto.ipc_definitions.AcquisitionCompletionStateReply>}
 */
const methodInfo_GetAcquisitionCompletionStateStream = new grpc.web.AbstractClientBase.MethodInfo(
  proto.ipc_definitions.AcquisitionCompletionStateReply,
  /** @param {!proto.google.protobuf.Empty} request */
  function(request) {
    return request.serializeBinary();
  },
  proto.ipc_definitions.AcquisitionCompletionStateReply.deserializeBinary
);


/**
 * @param {!proto.google.protobuf.Empty} request The request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @return {!grpc.web.ClientReadableStream<!proto.ipc_definitions.AcquisitionCompletionStateReply>}
 *     The XHR Node Readable Stream
 */
proto.ipc_definitions.AcquisitionManagerServiceClient.prototype.getAcquisitionCompletionStateStream =
    function(request, metadata) {
  return this.client_.serverStreaming(this.hostname_ +
      '/ipc_definitions.AcquisitionManagerService/GetAcquisitionCompletionStateStream',
      request,
      metadata,
      methodInfo_GetAcquisitionCompletionStateStream);
};


/**
 * @param {!proto.google.protobuf.Empty} request The request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @return {!grpc.web.ClientReadableStream<!proto.ipc_definitions.AcquisitionCompletionStateReply>}
 *     The XHR Node Readable Stream
 */
proto.ipc_definitions.AcquisitionManagerServicePromiseClient.prototype.getAcquisitionCompletionStateStream =
    function(request, metadata) {
  return this.delegateClient_.client_.serverStreaming(this.delegateClient_.hostname_ +
      '/ipc_definitions.AcquisitionManagerService/GetAcquisitionCompletionStateStream',
      request,
      metadata,
      methodInfo_GetAcquisitionCompletionStateStream);
};


module.exports = proto.ipc_definitions;

