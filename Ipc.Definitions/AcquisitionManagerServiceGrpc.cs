// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: AcquisitionManagerService.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Ipc.Definitions {
  public static partial class AcquisitionManagerService
  {
    static readonly string __ServiceName = "ipc_definitions.AcquisitionManagerService";

    static readonly grpc::Marshaller<global::Ipc.Definitions.StartRequest> __Marshaller_ipc_definitions_StartRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ipc.Definitions.StartRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ipc.Definitions.StartReply> __Marshaller_ipc_definitions_StartReply = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ipc.Definitions.StartReply.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ipc.Definitions.StopRequest> __Marshaller_ipc_definitions_StopRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ipc.Definitions.StopRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ipc.Definitions.StopReply> __Marshaller_ipc_definitions_StopReply = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ipc.Definitions.StopReply.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Google.Protobuf.WellKnownTypes.Empty> __Marshaller_google_protobuf_Empty = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Protobuf.WellKnownTypes.Empty.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ipc.Definitions.CurrentSampleNameReply> __Marshaller_ipc_definitions_CurrentSampleNameReply = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ipc.Definitions.CurrentSampleNameReply.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ipc.Definitions.AcquisitionStateReply> __Marshaller_ipc_definitions_AcquisitionStateReply = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ipc.Definitions.AcquisitionStateReply.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Ipc.Definitions.AcquisitionCompletionStateReply> __Marshaller_ipc_definitions_AcquisitionCompletionStateReply = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Ipc.Definitions.AcquisitionCompletionStateReply.Parser.ParseFrom);

    static readonly grpc::Method<global::Ipc.Definitions.StartRequest, global::Ipc.Definitions.StartReply> __Method_Start = new grpc::Method<global::Ipc.Definitions.StartRequest, global::Ipc.Definitions.StartReply>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Start",
        __Marshaller_ipc_definitions_StartRequest,
        __Marshaller_ipc_definitions_StartReply);

    static readonly grpc::Method<global::Ipc.Definitions.StopRequest, global::Ipc.Definitions.StopReply> __Method_Stop = new grpc::Method<global::Ipc.Definitions.StopRequest, global::Ipc.Definitions.StopReply>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Stop",
        __Marshaller_ipc_definitions_StopRequest,
        __Marshaller_ipc_definitions_StopReply);

    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Ipc.Definitions.CurrentSampleNameReply> __Method_GetCurrentSampleName = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Ipc.Definitions.CurrentSampleNameReply>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetCurrentSampleName",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_ipc_definitions_CurrentSampleNameReply);

    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Ipc.Definitions.CurrentSampleNameReply> __Method_GetCurrentSampleNameStream = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Ipc.Definitions.CurrentSampleNameReply>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "GetCurrentSampleNameStream",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_ipc_definitions_CurrentSampleNameReply);

    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Ipc.Definitions.AcquisitionStateReply> __Method_GetAcquisitionState = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Ipc.Definitions.AcquisitionStateReply>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetAcquisitionState",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_ipc_definitions_AcquisitionStateReply);

    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Ipc.Definitions.AcquisitionStateReply> __Method_GetAcquisitionStateStream = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Ipc.Definitions.AcquisitionStateReply>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "GetAcquisitionStateStream",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_ipc_definitions_AcquisitionStateReply);

    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Ipc.Definitions.AcquisitionCompletionStateReply> __Method_GetAcquisitionCompletionState = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Ipc.Definitions.AcquisitionCompletionStateReply>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetAcquisitionCompletionState",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_ipc_definitions_AcquisitionCompletionStateReply);

    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Ipc.Definitions.AcquisitionCompletionStateReply> __Method_GetAcquisitionCompletionStateStream = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Ipc.Definitions.AcquisitionCompletionStateReply>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "GetAcquisitionCompletionStateStream",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_ipc_definitions_AcquisitionCompletionStateReply);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Ipc.Definitions.AcquisitionManagerServiceReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of AcquisitionManagerService</summary>
    public abstract partial class AcquisitionManagerServiceBase
    {
      public virtual global::System.Threading.Tasks.Task<global::Ipc.Definitions.StartReply> Start(global::Ipc.Definitions.StartRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Ipc.Definitions.StopReply> Stop(global::Ipc.Definitions.StopRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Ipc.Definitions.CurrentSampleNameReply> GetCurrentSampleName(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task GetCurrentSampleNameStream(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::IServerStreamWriter<global::Ipc.Definitions.CurrentSampleNameReply> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Ipc.Definitions.AcquisitionStateReply> GetAcquisitionState(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task GetAcquisitionStateStream(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::IServerStreamWriter<global::Ipc.Definitions.AcquisitionStateReply> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Ipc.Definitions.AcquisitionCompletionStateReply> GetAcquisitionCompletionState(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task GetAcquisitionCompletionStateStream(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::IServerStreamWriter<global::Ipc.Definitions.AcquisitionCompletionStateReply> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for AcquisitionManagerService</summary>
    public partial class AcquisitionManagerServiceClient : grpc::ClientBase<AcquisitionManagerServiceClient>
    {
      /// <summary>Creates a new client for AcquisitionManagerService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public AcquisitionManagerServiceClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for AcquisitionManagerService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public AcquisitionManagerServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected AcquisitionManagerServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected AcquisitionManagerServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::Ipc.Definitions.StartReply Start(global::Ipc.Definitions.StartRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Start(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Ipc.Definitions.StartReply Start(global::Ipc.Definitions.StartRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Start, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Ipc.Definitions.StartReply> StartAsync(global::Ipc.Definitions.StartRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return StartAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Ipc.Definitions.StartReply> StartAsync(global::Ipc.Definitions.StartRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Start, null, options, request);
      }
      public virtual global::Ipc.Definitions.StopReply Stop(global::Ipc.Definitions.StopRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Stop(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Ipc.Definitions.StopReply Stop(global::Ipc.Definitions.StopRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Stop, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Ipc.Definitions.StopReply> StopAsync(global::Ipc.Definitions.StopRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return StopAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Ipc.Definitions.StopReply> StopAsync(global::Ipc.Definitions.StopRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Stop, null, options, request);
      }
      public virtual global::Ipc.Definitions.CurrentSampleNameReply GetCurrentSampleName(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetCurrentSampleName(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Ipc.Definitions.CurrentSampleNameReply GetCurrentSampleName(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetCurrentSampleName, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Ipc.Definitions.CurrentSampleNameReply> GetCurrentSampleNameAsync(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetCurrentSampleNameAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Ipc.Definitions.CurrentSampleNameReply> GetCurrentSampleNameAsync(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetCurrentSampleName, null, options, request);
      }
      public virtual grpc::AsyncServerStreamingCall<global::Ipc.Definitions.CurrentSampleNameReply> GetCurrentSampleNameStream(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetCurrentSampleNameStream(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncServerStreamingCall<global::Ipc.Definitions.CurrentSampleNameReply> GetCurrentSampleNameStream(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_GetCurrentSampleNameStream, null, options, request);
      }
      public virtual global::Ipc.Definitions.AcquisitionStateReply GetAcquisitionState(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetAcquisitionState(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Ipc.Definitions.AcquisitionStateReply GetAcquisitionState(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetAcquisitionState, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Ipc.Definitions.AcquisitionStateReply> GetAcquisitionStateAsync(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetAcquisitionStateAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Ipc.Definitions.AcquisitionStateReply> GetAcquisitionStateAsync(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetAcquisitionState, null, options, request);
      }
      public virtual grpc::AsyncServerStreamingCall<global::Ipc.Definitions.AcquisitionStateReply> GetAcquisitionStateStream(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetAcquisitionStateStream(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncServerStreamingCall<global::Ipc.Definitions.AcquisitionStateReply> GetAcquisitionStateStream(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_GetAcquisitionStateStream, null, options, request);
      }
      public virtual global::Ipc.Definitions.AcquisitionCompletionStateReply GetAcquisitionCompletionState(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetAcquisitionCompletionState(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Ipc.Definitions.AcquisitionCompletionStateReply GetAcquisitionCompletionState(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetAcquisitionCompletionState, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Ipc.Definitions.AcquisitionCompletionStateReply> GetAcquisitionCompletionStateAsync(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetAcquisitionCompletionStateAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Ipc.Definitions.AcquisitionCompletionStateReply> GetAcquisitionCompletionStateAsync(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetAcquisitionCompletionState, null, options, request);
      }
      public virtual grpc::AsyncServerStreamingCall<global::Ipc.Definitions.AcquisitionCompletionStateReply> GetAcquisitionCompletionStateStream(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetAcquisitionCompletionStateStream(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncServerStreamingCall<global::Ipc.Definitions.AcquisitionCompletionStateReply> GetAcquisitionCompletionStateStream(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_GetAcquisitionCompletionStateStream, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override AcquisitionManagerServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new AcquisitionManagerServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(AcquisitionManagerServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Start, serviceImpl.Start)
          .AddMethod(__Method_Stop, serviceImpl.Stop)
          .AddMethod(__Method_GetCurrentSampleName, serviceImpl.GetCurrentSampleName)
          .AddMethod(__Method_GetCurrentSampleNameStream, serviceImpl.GetCurrentSampleNameStream)
          .AddMethod(__Method_GetAcquisitionState, serviceImpl.GetAcquisitionState)
          .AddMethod(__Method_GetAcquisitionStateStream, serviceImpl.GetAcquisitionStateStream)
          .AddMethod(__Method_GetAcquisitionCompletionState, serviceImpl.GetAcquisitionCompletionState)
          .AddMethod(__Method_GetAcquisitionCompletionStateStream, serviceImpl.GetAcquisitionCompletionStateStream).Build();
    }

  }
}
#endregion
