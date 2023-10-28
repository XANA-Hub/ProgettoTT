using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleEffectsManager : MonoBehaviour {


    [Header("Battle effect overlays")]
    public Image gotHitOverlay;
    public Image healOverlay;
    public Image defendOverlay;
    public Image defeatOverlay;


    private Color gotHitOverlayOriginalColor;
    private Color healOverlayOriginalColor;
    private Color defendOverlayOriginalColor;


    private void Start() {
        gotHitOverlayOriginalColor = gotHitOverlay.color;
        healOverlayOriginalColor = healOverlay.color;
        defendOverlayOriginalColor = defendOverlay.color;
        
        gotHitOverlay.color = new Color(gotHitOverlayOriginalColor.r, gotHitOverlayOriginalColor.g, gotHitOverlayOriginalColor.b, 0f);
        healOverlay.color = new Color(healOverlayOriginalColor.r, healOverlayOriginalColor.g, healOverlayOriginalColor.b, 0f);
        defendOverlay.color = new Color(defendOverlayOriginalColor.r, defendOverlayOriginalColor.g, defendOverlayOriginalColor.b, 0f);
    }

    public void ShowOverlay(BattleEffect effect) {
        
        if(effect == BattleEffect.PLAYER_DEFEAT) {
            StartCoroutine(ShowDefeatOverlay());
        } else {
            StartCoroutine(ShowAndHideOverlay(effect));
        }

    }

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

    private IEnumerator ShowAndHideOverlay(BattleEffect effect) {

        Image overlayImage;
        Color originalColor;

        if (effect == BattleEffect.ENEMY_ATTACK) {
            overlayImage = gotHitOverlay;
            originalColor = gotHitOverlayOriginalColor;
        }
        else if (effect == BattleEffect.PLAYER_HEAL) {
            overlayImage = healOverlay;
            originalColor = healOverlayOriginalColor;
        }
        else if (effect == BattleEffect.PLAYER_DEFEND) {
            overlayImage = defendOverlay;
            originalColor = defendOverlayOriginalColor;
        }
        else {
            // Ritorna immediatamente se l'effetto non è gestito
            Debug.LogError("BattleEffectsManager: Effetto non trovato!");
            yield break;
        }

        float fadeDuration = 0.5f;
        float elapsedTime = 0f;

        // Show the overlay
        while (elapsedTime < fadeDuration) {
            float alpha = Mathf.Lerp(0f, 1.0f, elapsedTime / fadeDuration);
            overlayImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Wait for a second
        yield return new WaitForSeconds(0.5f);

        // Hide the overlay
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration) {
            float alpha = Mathf.Lerp(1.0f, 0f, elapsedTime / fadeDuration);
            overlayImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Assicurati che l'overlay sia completamente nascosto
        overlayImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }

    
    private IEnumerator ShowDefeatOverlay() {

        float fadeDuration = 2.0f;
        float elapsedTime = 0f;

        // Show the overlay
        while (elapsedTime < fadeDuration) {
            float alpha = Mathf.Lerp(0f, 1.0f, elapsedTime / fadeDuration);
            defeatOverlay.color = new Color(defeatOverlay.color.r, defeatOverlay.color.g, defeatOverlay.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }
    

    
    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.gameObject.SetActive(false);
    }
}
