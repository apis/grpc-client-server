using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Ipc.Server
{
	public class MyInterceptor : Interceptor
	{
		private static void AddAccessControlAllowOriginHeader(Metadata metadata)
		{
			metadata.Add("Access-Control-Allow-Origin", "*");
		}
//
//		public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
//		{
//			AddAccessControlAllowOriginHeader(context.Options.Headers);
//			return continuation(request, context);
//		}
//
//		public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
//		{
//			AddAccessControlAllowOriginHeader(context.Options.Headers);
//			return continuation(request, context);
//		}
//		
//		public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
//		{
//			AddAccessControlAllowOriginHeader(context.Options.Headers);
//			return continuation(request, context);
//		}
//		
//		public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
//		{
//			AddAccessControlAllowOriginHeader(context.Options.Headers);
//			return continuation(context);
//		}
//		
//		public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
//		{
//			AddAccessControlAllowOriginHeader(context.Options.Headers);
//			return continuation(context);
//		}
		
		public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
		{
			AddAccessControlAllowOriginHeader(context.RequestHeaders);
			return continuation(request, context);
		}
		
		public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation) 
		{
			AddAccessControlAllowOriginHeader(context.RequestHeaders);
			return continuation(requestStream, context);
		}
		
		public override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation) 
		{
			AddAccessControlAllowOriginHeader(context.RequestHeaders);
			return continuation(request, responseStream, context);
		}
		
		public override Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation) 
		{
			AddAccessControlAllowOriginHeader(context.RequestHeaders);
			return continuation(requestStream, responseStream, context);
		}
	}
}