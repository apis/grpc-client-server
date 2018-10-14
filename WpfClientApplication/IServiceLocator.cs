namespace WpfClientApplication
{
	public interface IServiceLocator
	{
		T GetService<T>();

		void RegisterService<T>(object instance);
	}
}