using System.Collections;
using PartyFinderToolbox.Shared.Services;
using PartyFinderToolbox.Shared.Services.Task;

public class CoroutineManager
{
    // 実行中のコルーチンを保持するリスト
    private readonly List<IEnumerator> _coroutines = [];

    // コルーチンを開始する
    public void Start(IEnumerator routine)
    {
        if (!_coroutines.Contains(routine)) _coroutines.Add(routine);
    }

    // コルーチンを停止する
    public void Stop(IEnumerator routine)
    {
        _coroutines.Remove(routine);
    }

    /// <summary>
    ///     毎フレーム(または一定周期)呼び出す想定の更新メソッド
    ///     deltaTime は実行環境に合わせて渡す(1フレームの経過時間など)
    /// </summary>
    public void Update()
    {
        // 終了が確定したコルーチンを一時的に格納
        List<IEnumerator> finishedCoroutines = [];

        // 登録されているコルーチンを順番に更新
        // forではなく、foreachで回すと走査中の要素削除が難しいので注意
        for (var i = 0; i < _coroutines.Count; i++)
        {
            var routine = _coroutines[i];

            // 現在の要素を1ステップ進める
            var running = AdvanceCoroutine(routine);

            // 進行できなかった(=コルーチンが終了した)場合
            if (!running)
            {
                finishedCoroutines.Add(routine);
            }
        }

        // 終了したコルーチンをリストから削除
        if (finishedCoroutines.Count > 0)
            foreach (var c in finishedCoroutines)
                _coroutines.Remove(c);
    }

    /// <summary>
    ///     IEnumerator(コルーチン相当)を1ステップ進める。
    ///     戻り値が false の場合はコルーチン終了とみなす。
    /// </summary>
    private static bool AdvanceCoroutine(IEnumerator routine)
    {
        var current = routine.Current;
        
        if (current is IYieldInstruction {KeepWaiting:true} instruction)
        {
            return true;
        }
        
        Logger.Warning($"次へ", true);
        
        // 次のステップに進む
        return routine.MoveNext();
    }
}