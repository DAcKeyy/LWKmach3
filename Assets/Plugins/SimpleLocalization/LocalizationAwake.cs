using UnityEngine;

namespace Assets.SimpleLocalization
{
	public class LocalizationAwake : MonoBehaviour
	{
		public void Awake()
		{
			LocalizationManager.Read();

			switch (Application.systemLanguage)
			{
				case SystemLanguage.German:
					LocalizationManager.Language = "German";
					break;
				case SystemLanguage.Russian:
					LocalizationManager.Language = "Russian";
					break;
				default:
					LocalizationManager.Language = "English";
					break;
			}
		}

		/// <summary>
		/// Change localization at runtime
		/// </summary>
		public void SetLocalization(string localization)
		{
			LocalizationManager.Language = localization;
		}
	}
}