using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NotifylSlider : MonoBehaviour
{
	public TextMeshProUGUI notiText;
	public float moveSpeed = 100f; // Tốc độ di chuyển
	public Vector2 startPosition; // Vị trí bắt đầu của Text
	public Vector2 targetPosition; // Vị trí đích của Text
	public float hideDelay = 1f; // Thời gian chờ trước khi Text biến mất
	public void Inject(string msg)
	{
		notiText.text = msg;
		MakeSlide();
	}
	public void MakeSlide()
	{
		this.gameObject.SetActive(true);
		StartCoroutine(MoveText());
	}
	IEnumerator MoveText()
	{
		// Di chuyển Text từ vị trí bắt đầu đến vị trí đích
		while (Vector2.Distance(notiText.rectTransform.anchoredPosition, targetPosition) > 0.1f)
		{
			notiText.rectTransform.anchoredPosition = Vector2.MoveTowards(
				notiText.rectTransform.anchoredPosition,
				targetPosition,
				moveSpeed * Time.deltaTime
			);
			yield return null;
		}

		// Chờ một chút trước khi biến mất
		yield return new WaitForSeconds(hideDelay);

		// Ẩn Text
		gameObject.SetActive(false);

		// Chờ một chút trước khi đặt lại vị trí ban đầu và hiện lại Text
		yield return new WaitForSeconds(1f);
		notiText.rectTransform.anchoredPosition = startPosition;
	}

}
