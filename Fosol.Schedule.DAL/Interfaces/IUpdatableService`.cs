using System.Collections.Generic;

namespace Fosol.Schedule.DAL.Interfaces
{
	public interface IUpdatableService<TModel> : IUpdatableService<TModel, TModel, TModel, TModel>
		where TModel : class
	{

	}

	/// <summary>
	/// IUpdatableService interface, provides generic functions to update the datasource.
	/// </summary>
	/// <typeparam name="TCreate"></typeparam>
	/// <typeparam name="TRead"></typeparam>
	/// <typeparam name="TUpdate"></typeparam>
	/// <typeparam name="TDelete"></typeparam>
	public interface IUpdatableService<TCreate, TRead, TUpdate, TDelete> : IReadableService<TRead>
		where TCreate : class
		where TRead : class
		where TUpdate : class
		where TDelete : class
	{
		#region Methods
		/// <summary>
		/// Add the model to the in-memory collection so that it can be saved to the datasource.
		/// </summary>
		/// <param name="model"></param>
		TUpdate Add(TCreate model);

		/// <summary>
		/// Add the models to the in-memory collection so that they can be saved to the datasource.
		/// </summary>
		/// <param name="models"></param>
		IEnumerable<TUpdate> AddRange(IEnumerable<TCreate> models);

		/// <summary>
		/// Add the models to the in-memory collection so that they can be saved to the datasource.
		/// </summary>
		/// <param name="models"></param>
		IEnumerable<TUpdate> AddRange(params TCreate[] models);

		/// <summary>
		/// Remove the model from the in-memory collection so that it can be deleted from the datasource.
		/// </summary>
		/// <param name="model"></param>
		void Remove(TDelete model);

		/// <summary>
		/// Remove the models from the in-memory collection so that they can be deleted from the datasource.
		/// </summary>
		/// <param name="models"></param>
		void RemoveRange(IEnumerable<TDelete> models);

		/// <summary>
		/// Remove the models from the in-memory collection so that they can be deleted from the datasource.
		/// </summary>
		/// <param name="models"></param>
		void RemoveRange(params TDelete[] models);

		/// <summary>
		/// Update the model in the in-memory collection so that it can be saved to the datasource.
		/// </summary>
		/// <param name="model"></param>
		TUpdate Update(TUpdate model);

		/// <summary>
		/// Update the models in the in-memory collection so that they can be saved to the datasource.
		/// </summary>
		/// <param name="models"></param>
		IEnumerable<TUpdate> UpdateRange(IEnumerable<TUpdate> models);

		/// <summary>
		/// Update the models in the in-memory collection so that they can be saved to the datasource.
		/// </summary>
		/// <param name="models"></param>
		IEnumerable<TUpdate> UpdateRange(params TUpdate[] models);
		#endregion
	}
}