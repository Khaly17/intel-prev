using System.Threading.Tasks;
using AutoMapper;
using Soditech.IntelPrev.Mobile.Core.DataStorage;
using Soditech.IntelPrev.Mobile.Models.Common;
using Soditech.IntelPrev.Mobile.Services.Account.Models;

namespace Soditech.IntelPrev.Mobile.Services.Storage;

public class DataStorageService : IDataStorageService
{
	private readonly IDataStorageManager _dataStorageManager;
	private readonly IMapper _objectMapper;

	public DataStorageService(
		IDataStorageManager dataStorageManager,
		IMapper objectMapper)
	{
		_dataStorageManager = dataStorageManager;
		_objectMapper = objectMapper;
	}

	public async Task StoreAccessTokenAsync(string newAccessToken, string newEncryptedAccessToken)
	{
		var authenticateResult = _dataStorageManager.Retrieve<AuthenticateResultModel>(DataStorageKey.CurrentSession_TokenInfo);

		authenticateResult.AccessToken = newAccessToken;
		authenticateResult.EncryptedAccessToken = newEncryptedAccessToken;

		await _dataStorageManager.StoreAsync(DataStorageKey.CurrentSession_TokenInfo, authenticateResult);
	}

	public AuthenticateResultModel RetrieveAuthenticateResult()
	{
		var data = _dataStorageManager.Retrieve<AuthenticateResultModel>(
			DataStorageKey.CurrentSession_TokenInfo
		);

		return _objectMapper.Map<AuthenticateResultModel>(
			data
		);
	}

	public async Task StoreAuthenticateResultAsync(AuthenticateResultModel? authenticateResultModel)
	{
		await _dataStorageManager.StoreAsync(
			DataStorageKey.CurrentSession_TokenInfo,
			_objectMapper.Map<AuthenticateResultModel>(authenticateResultModel)
		);
	}



	public UserLoginInfoPersistanceModel RetrieveLoginInfo()
	{
		return _objectMapper.Map<UserLoginInfoPersistanceModel>(
			_dataStorageManager.Retrieve<CurrentLoginInformationPersistanceModel>(
				DataStorageKey.CurrentSession_LoginInfo
			)
		);
	}

	public async Task StoreLoginInformationAsync(UserLoginInfoPersistanceModel loginInfo)
	{
		await _dataStorageManager.StoreAsync(
			DataStorageKey.CurrentSession_LoginInfo,
			_objectMapper.Map<CurrentLoginInformationPersistanceModel>(
				loginInfo
			)
		);
	}

	public void ClearSessionPersistance()
	{
		_dataStorageManager.RemoveIfExists(DataStorageKey.CurrentSession_TokenInfo);
		_dataStorageManager.RemoveIfExists(DataStorageKey.CurrentSession_LoginInfo);
	}

	// Settings storage methods
	public T GetValueOrDefault<T>(string key, T defaultValue = default)
	{
		return _dataStorageManager.Retrieve<T>(key, defaultValue);
	}

	public async Task SetValue<T>(string key, T value)
	{
		await _dataStorageManager.StoreAsync(key, value);
	}

	public bool HasValue(string key)
	{
		return _dataStorageManager.HasKey(key);
	}

	public void RemoveValue(string key)
	{
		_dataStorageManager.RemoveIfExists(key);
	}
}