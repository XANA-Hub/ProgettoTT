using System.Collections;
using UnityEngine;

public class BattleEffectsManager : MonoBehaviour {

    public void ShakeCamera(Camera camera, float duration, float magnitude) {
        StartCoroutine(Shake(camera.gameObject, duration, magnitude));
    }

    public void ShakeGameObject(GameObject gameObject, float duration, float magnitude) {
        StartCoroutine(Shake(gameObject, duration, magnitude));
    }

    private IEnumerator Shake(GameObject gameObject, float duration, float magnitude) {

        Vector3 originalPosition = gameObject.transform.position;
        float elapsed = 0f;

        while (elapsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            gameObject.transform.position = new Vector3(x, y, -10f);
            elapsed += Time.deltaTime;

            yield return null;
        }

        gameObject.transform.position = originalPosition;
    }



    
    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.gameObject.SetActive(false);
    }
}
