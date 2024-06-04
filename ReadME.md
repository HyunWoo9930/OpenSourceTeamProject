# 오픈소스S/W 2팀 github 레포지토리

## 명단

#### 조장 : 오현우 / 201802265 / 전자물리학과

##### 팀원 :

1. 허준재 / 202103686 / 컴퓨터공학부
2. 구예빈 / 202303475 / 컴퓨터공학부
3. 이동재 / 201802493 / 전자물리학과

## 디자인

1. 홈, 인게임, 설정
2. 5월 첫째주까지
3. Boo 캐릭터 추가, 애니메이션 추가.

## 함수 설명

1. ﻿ButtonController:
    버튼을 클릭하는 기능을 처리하는 클래스를 정의합니다.
    Start 함수는 게임 object가 활성화되면 호출되는 함수입니다. Start()의 기능은 버튼이 클릭되면 TaskOnClick 메서드를 실행하는 것입니다.
    TaskOnClick 함수는 버튼이 Replay이거나 Play이면 InGameScene이 실행되고, Home이면 MainScene이 실행되는 것입니다.

2. ﻿CharacterManager 클래스:
    캐릭터와 관련된 기능을 할 수 있는 클래스를 정의합니다.
    캐릭터의 인덱스를 읽기 위해 index를 선언하고, 캐릭터 이름들을 배열로 정의하고, DSLManager로 캐릭터의 정보와 설정들을 가져오고, 버튼, 소리, 이미지, 이름 보여주는 변수들을 선언합니다.<br>
<br>
 1) ﻿Awake()는 선택된 캐릭터 인덱스를 가져와서 초기화하고, 오디오 소스를 가져와서 소리를 키거나 SoundBtn이 눌리면 음소거가 되는 코드를 추가하고, ArrowBtn을 초기화해줍니다.<br>
 2) ﻿ArrowBtn()은 버튼에 따라 방향을 결정하는 코드입니다. 만약 dir이 Right이면 인덱스를 하나 증가해주고 dir이 Left이면 인텍스를 하나 감소합니다. 또한 캐릭터 이미지와 이름을 업데이트 하며 버튼 활성화 상태를 결정해줍니다.<br>
 3) ﻿Character 클래스<br>
 캐릭터의 영어, 한글 이름을 전언하고, 선택여부를 설정하고 이를 배열로 나타냅니다.<br>
 4)﻿Inform 클래스<br>
 Inform클래스는 게임 설정란에 설정기능을 담당합니다. 에이쁠이 보2조는 설정란에 소리 on, off, 진동 기능을 추가했습니다. Inform()은 게임이 처음 시작될 때, 데이터를 로드 할때 등 현재 상태를 결정하는 함수입니다.<br>
 5)﻿ Ranking 클래스<br>
 점수와 캐릭터의 index를 빋아와서 캐릭터가 몇 점을 가지고 있는지 순위 정보를 저장하는 클래스입니다.<br>


3. ﻿DSLManager 클래스
 캐릭터, 랭킹, 설정 등 배열에 있는 정보 값들을 저장합니다. 
또한, gameManager, characterManager은 각각 다른 gameManager, characterManager 클래스를 참조하고, characterSprite은 캐릭터 이미지 배열을 저장하고, rankCharacterImg은 순위에 따라 캐릭터를 나열하는 배열입니다.

﻿ 1) Awake()에선 게임이 실행될 때 저장된 데이터로 캐릭터, 순위, 설정 등을 초기화하고 초기 데이터를 플레이어에게 제공하는 함수입니다.
 ﻿2)DataSave 함수는 Base64로 인코딩하는 함수이고, DataLoad함수는 Base64로 디코딩하는 함수입니다.
 ﻿3)SaveCharacterIndex함수는 캐릭터의 인덱스를 저장하고, 씬을 로드하는 기능을 하고,  GetSelectedCharIndex함수는 데이터를 로드해온 후 현재 선택된 캐릭터의 인덱스를 반환하는 기능을 합니다.
﻿ 4) IsRetry 함수는 informs[0].Retry값이 true이면 다시 시작하고, false이면 작동하지 않습니다.
﻿ 5) ChangeRetry 함수는 다시 시작할 때 변경된 데이터를 기반으로 다시 시작하는 것을 변경합니다.
﻿ 6) LoadRanking 함수는 랭킹을 저장하고, SaveRankScore 함수는 점수에 따라 내림차순으로 점수를 정렬하는 함수입니다.
 7)  GetBestScore 함수는 최고기록을 반환하는 함수입니다.
﻿ 8) GetSettingOn 함수는 설정에 있는 소리 버튼들을 처음으로 초기화하는 함수입니다.
 9) ChangeOnOff함수는 클릭하는 기능에 따라 소리 관련 기능을 변경하는 함수입니다.
 ﻿10) OnApplicationQuit함수는 앱을 종료하는 함수이고, OnApplicationPause함수는 앱이 일시정지된 상황을 해결하는 함수입니다.


4. ﻿DontDestroy.cs
 1) DontDestroy 클래스
 싱글톤 패턴을 사용하기 위해 Instance 변수를 사용합니다. Instace가 존재하면 게임오브젝트를 Destroy함수를 이용하여 없애고, Instance가 없는 경우 DontDestroyOnLoad함수를 실행합니다.
 ﻿2) BgmPlay함수는 Instance가 null인경우와 null이 아닌 경우로 나눠서 bgm을 재생합니다.
 3)﻿BgmStop함수는 브금을 멈추는 함수입니다. if-else구문을 사용하여 현재 사용하고 있는 브금을 가져온 뒤 bgm.enabled를 false로 변경하며 브금을 정지합니다.

5. ﻿GameManagers.cs
   1)GameManager 클래스는 게임을 실행할 때 사용해야 하는 모든 기능(플레이어, 계단, UI 등)들을 불러옵니다.
   2) ﻿Awake()가 실행될 때 계단 초기화, 게이지, UI활성화 등을 해주고, Update()를 실행할 때 if절에 올라가는 버튼, else if문에 방향전환 버튼을 실행해줍니다.
   3) ﻿StairsInit()은 계단을 초기에 설정하는 함수이고, SpawnStair()은 계단을 생성하는 함수입니다. StairMove()함수는 계단이 생성될때 계단을 이동해주는 코드와, 올바르지 않은 방향으로 캐릭터가 이동하면 게임을 끝내는 기능을 맡고 있습니다. 또한 이 함수에는 계단을 이동하면 게이지가 증가하는 코드도 포함되어 있습니다.
   4) ﻿GaugeReduce()은 점수에 따라 게이지가 감소하는 비율을 코드로 구현한 함수이고, CheckGauge()은 게이지가 0이 되면 게임을 종료해주는 함수입니다.
   5) ﻿GameOver()은 게임 종료 화면으로 이동했을 때 나타나야 하는 화면과 플레이어 포즈를 구현하고, 최종 점수를 나타냅니다. 그리고 필요없는 버튼들과 기능들은 비활성화 시켜줍니다.
   6) ﻿ShowScore()은 최종 점수를 나타내는 함수입니다
   7) ﻿BtnDown함수와 BtnUp함수는 버튼을 눌렀을 때와 버튼을 땠을 때 상태를 나타내는 기능을 수행합니다.
   8) ﻿SoundInit(), SettingBtninit()은 소리와 버튼을 초기화하는 함수입니다.
   9) ﻿SettingBtnChange함수는 설정 버튼 상태가 변경될 때 호출되는 함수입니다. 버튼이 눌리면 Button이 전달되게 합니다.
   10) ﻿SettingOnOff함수는 배경음악, 소리, 진동 등을 켜거나 끄는 기능을 하는 함수입니다.
   11) ﻿Vibration함수는 진동을 발생하게 합니다.
   12) ﻿PlaySound함수는 특정 인덱스 또는 게임에서 필요한 사운드를 재생하는 함수입니다.
   13) ﻿DisableUI함수는 UI를 비활성화하는 함수입니다.
   ﻿14) LoadScene()은 특정 씬으로 넘어가야 할 때 사용하는 함수입니다.
   15) ﻿OnApplicationQuit()은 앱을 종료할 때 사용하는 함수입니다.


