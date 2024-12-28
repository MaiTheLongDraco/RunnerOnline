using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]
public class LinkHandlerTMPText : MonoBehaviour, IPointerClickHandler
{
	//private TMP_Text _tmpText;
	//[SerializeField] private Canvas _canvasCheck;
	//[SerializeField] private Camera cameraToUse;
	private Action<TMP_LinkInfo,Vector3> _linkHandler;
	//private void Awake()
	//{
	//	_tmpText = GetComponent<TMP_Text>();

	//}

	//private void Start()
	//{

	//	//  _canvasCheck = _iMainUi?.GetChildElement<UiTrungTam>().GetComponent<Canvas>();

	//	if (_canvasCheck.renderMode == RenderMode.ScreenSpaceOverlay)
	//	{
	//		cameraToUse = null;
	//	}
	//	else
	//	{
	//		cameraToUse = _canvasCheck.worldCamera;
	//	}
	//}

	public void Inject(Action<TMP_LinkInfo,Vector3> linkHandler)
	{
		_linkHandler = linkHandler;
	}

	//public void OnPointerClick(PointerEventData eventData)
	//{
	//	Vector3 mousePosition = new Vector3(eventData.position.x, eventData.position.y, 0);
	//	var linkTagetText = TMP_TextUtilities.FindIntersectingLink(_tmpText, mousePosition, cameraToUse);
	//	if (linkTagetText != -1)
	//	{
	//		TMP_LinkInfo linkInfo = _tmpText.textInfo.linkInfo[linkTagetText];
	//		Debug.Log($"link info ID {linkInfo.GetLinkID()} link text {linkInfo.GetLinkText()}");
	//		_linkHandler?.Invoke(linkInfo, mousePosition);

	//	}
	//}
	public TextMeshProUGUI textMeshPro; // TextMeshPro UGUI component
	private float lastClickTime = 0f; // Thời gian của lần click trước
	private const float doubleClickThreshold = 0.25f; // Thời gian tối đa giữa 2 lần click để được tính là double-click

	public void OnPointerClick(PointerEventData eventData)
	{
		// Kiểm tra nếu người dùng double-click
		if (Time.time - lastClickTime < doubleClickThreshold)
		{
			int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, Input.mousePosition, null);
			if (linkIndex != -1)
			{
				// Lấy thông tin của link
				Vector3 mousePosition = new Vector3(eventData.position.x, eventData.position.y, 0);
				TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
				_linkHandler?.Invoke(linkInfo, mousePosition);
			}
		}
		// Lưu thời gian click để kiểm tra double-click
		lastClickTime = Time.time;
	}
}

