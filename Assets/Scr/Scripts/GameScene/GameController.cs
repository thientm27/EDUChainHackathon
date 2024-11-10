using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Imba.UI;
using Imba.Utils;
using Newtonsoft.Json;
using Scr.Scripts.UI.Popups;
using Scr.Scripts.UI.View;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scr.Scripts.GameScene
{
    public class GameController : ManualSingletonMono<GameController>
    {
        [Header("Config")]
        [SerializeField] private float playerMoveSpeed;

        [SerializeField] private float enemyMoveSpeed;

        [Header("AxieControl")]
        [SerializeField] private GameObject demoAxieGroup;

        [SerializeField] private GameObject selectingAxieGroup;
        [SerializeField] private GameObject currentSelectAxieModel;

        [SerializeField] private SerializedDictionary<string, GameObject> axieModelConfig;

        [SerializeField] private Transform playerTf;

        private string _userAxieSelected;

        [Header("GameCore")]
        [SerializeField] private TextAsset jsonQuestionData;

        private List<QuestionData> _questionData;

        private QuizView _quizView;
        private int      _correctAns      = 0;
        private int      _currentQuestion = 0;
        private int      _userHeart       = 3;

        private void Start()
        {
            UIManager.Instance.ViewManager.ShowView(UIViewName.MainView);
            _quizView = UIManager.Instance.ViewManager.GetViewByName<QuizView>(UIViewName.QuizView);
        }

        public void InitGame()
        {
            _userHeart = 3;
            _quizView.SetHealth(_userHeart);
            ConvertJsonToQuestionData(jsonQuestionData.text);
            ShuffleQuestionBank();
            DisplayQuestion(_currentQuestion);

            // TO MOVE

            if (axieModelConfig.TryGetValue(_userAxieSelected, out var modelAxie))
            {
                if (currentSelectAxieModel)
                {
                    SimplePool.Despawn(currentSelectAxieModel);
                }

                currentSelectAxieModel =
                    SimplePool.Spawn(modelAxie, playerTf.transform.position, Quaternion.identity);
                currentSelectAxieModel.transform.SetParent(playerTf.transform);
                currentSelectAxieModel.transform.localScale    = Vector3.one * 0.5f;
                currentSelectAxieModel.transform.localRotation = Quaternion.identity;
            }
        }

        public void NextQuestion()
        {
            _currentQuestion++;
            if (_userHeart == 0)
            {
                Debug.Log("END GAME: DEAD");
                EndGame();
                return;
            }

            if (_currentQuestion == _questionData.Count)
            {
                Debug.Log("END GAME");
                EndGame();
                return;
            }

            DisplayQuestion(_currentQuestion);
        }

        public void ConvertJsonToQuestionData(string rawJson)
        {
            _questionData = JsonConvert.DeserializeObject<List<QuestionData>>(rawJson);
        }

        public void ShuffleQuestionBank()
        {
            for (int i = _questionData.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                (_questionData[i], _questionData[randomIndex]) = (_questionData[randomIndex], _questionData[i]);
            }
        }

        public void DisplayQuestion(int questionIndex)
        {
            _quizView.SetQuestionProcess(questionIndex + 1, _questionData.Count);
            var data = _questionData[questionIndex];
            _quizView.ShowQuestion(new()
            {
                question      = data.Question,
                answer1       = data.Options[0],
                answer2       = data.Options[1],
                answer3       = data.Options[2],
                answer4       = data.Options[3],
                correctAnswer = data.Options[data.Answer]
            });
        }

        public void ChooseWrongAnswer()
        {
            StartCoroutine(WrongAnsHandler());
        }

        public void ChooseCorrectAnswer()
        {
            StartCoroutine(CorrectAnsHandler());
        }

        public IEnumerator CorrectAnsHandler()
        {
            _correctAns++;
            yield return new WaitForSeconds(1.5f);
            _quizView.HideQuestion();
            isMoving = true;
            yield return new WaitForSeconds(0.5f);
            isMoving = false;
            NextQuestion();
            yield return null;
        }

        public IEnumerator WrongAnsHandler()
        {
            yield return new WaitForSeconds(1.5f);
            _quizView.HideQuestion();
            _userHeart--;
            _quizView.SetHealth(_userHeart);
            isMoving = true;
            yield return new WaitForSeconds(0.5f);
            isMoving = false;
            NextQuestion();
            yield return null;
        }

        public void EndGame()
        {
            var earn = 0.1f * _correctAns;
            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.EndGamePopup, new EndGameParam
            {
                tokenWin   = earn,
                correctAns = _correctAns,
                maxQuest   = _questionData.Count
            });
        }

        public void SetDemoAxieGroup(bool isActive)
        {
            demoAxieGroup.SetActive(isActive);
        }

        public void SetSelectingAxieGroup(bool isActive)
        {
            selectingAxieGroup.SetActive(isActive);
            if (!isActive)
            {
                if (currentSelectAxieModel)
                {
                    SimplePool.Despawn(currentSelectAxieModel);
                }
            }
        }

        public void SetSelectingAxie(string nameAxie)
        {
            _userAxieSelected = nameAxie;
            if (axieModelConfig.TryGetValue(nameAxie, out var modelAxie))
            {
                if (currentSelectAxieModel)
                {
                    SimplePool.Despawn(currentSelectAxieModel);
                }

                currentSelectAxieModel =
                    SimplePool.Spawn(modelAxie, selectingAxieGroup.transform.position, Quaternion.identity);
                currentSelectAxieModel.transform.SetParent(selectingAxieGroup.transform);
                currentSelectAxieModel.transform.localScale    = Vector3.one;
                currentSelectAxieModel.transform.localRotation = Quaternion.identity;
            }
            else
            {
                Debug.Log("NOT FOUND FOR: " + nameAxie);
            }
        }

        public bool isMoving;

        private void Update()
        {
            if (isMoving)
            {
                var newPos = playerTf.transform.position;
                newPos.x                    += playerMoveSpeed * Time.deltaTime;
                playerTf.transform.position =  newPos;
            }
        }
    }

    public class QuestionData
    {
        public int          QuestionId { get; set; }
        public string       Question   { get; set; }
        public List<string> Options    { get; set; }
        public int          Answer     { get; set; }
    }
}