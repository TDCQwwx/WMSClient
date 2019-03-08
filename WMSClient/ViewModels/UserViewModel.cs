using ClassLibraryHelper;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using SaeaServer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WMSClient.Common;
using WMSClient.Models;
using WMSClient.TcpIp;
using WMSClient.Views;

namespace WMSClient.ViewModels
{
    public class UserViewModel : NotificationObject
    {
        #region 安全的单例模式
        private static volatile UserViewModel _instanceUserViewModel;
        private static readonly object obj = new object();
        //private UserViewModel() { }
        public static UserViewModel InstanceUserViewModel
        {
            get
            {
                if (null == _instanceUserViewModel)
                {
                    lock (obj)
                    {
                        if (null == _instanceUserViewModel)
                        {
                            _instanceUserViewModel = new UserViewModel();
                        }
                    }
                }
                return _instanceUserViewModel;
            }
        }
        #endregion

        #region Fields
        //TcpIp
        private static TcpSocketSaeaClient _client;
        private TcpSocketSaeaClientConfiguration _clientConfig;
        private ClientEventDispatcher _clientDispatcher;

        private string operationMode = "online";//默认是联机模式

        #endregion

        #region Atrribute
        public DelegateCommand ConfirmOperationModeCommand { get; set; }//操作模式确认

        //WarehousePLC
        public DelegateCommand InputLoadTrayCommand { get; set; }//入库上料
        public DelegateCommand ReadRFIDCommand { get; set; }//读取RFID
        public DelegateCommand WriteRFIDCommand { get; set; }//写入RFID
        public DelegateCommand InputCommand { get; set; }//入库执行
        public DelegateCommand<object> OutputCommand { get; set; }//出库执行
        public DelegateCommand OutputUnloadTrayCommand { get; set; }//出库下料
        public DelegateCommand CreateRFIDCommand { get; set; }//生成RFID号

        //AGV
        public DelegateCommand AGVDispatchGetCurrentPositionCommand { get; set; }//AGV调度：获取当前坐标
        public DelegateCommand AGVDispatchMoveCommand { get; set; }//AGV调度：移动执行
        public DelegateCommand AGVDispatchLoadTrayCommand { get; set; }//AGV调度：进料盘任务
        public DelegateCommand AGVDispatchReleaseTrayCommand { get; set; }//AGV调度：放料盘任务

        //CloudPlatform
        public DelegateCommand StoragesQueryCommand { get; set; }//库位查询命令
        public DelegateCommand InputRecordQueryCommand { get; set; }//入库记录查询
        public DelegateCommand OutputRecordQueryCommand { get; set; }//出库记录查询
        public DelegateCommand ModifyStorageRecordQueryCommand { get; set; }//修改库位记录查询
        public DelegateCommand<object> EditStoragesInfoCommand { get; set; }//编辑库位信息

        //Model
        public UserModel UserModel { get; set; }//用户模型
        public InputModel InputModel { get; set; }//入库模型
        public OperationModeModel OperationModeModel { get; set; }//操作模式模型
        public OutputRecordModel OutputRecordModel { get; set; }//出库记录模型
        public StorageInfoModel StorageInfoModel { get; set; }//库位信息模型
        public InputRecordModel InputRecordModel { get; set; }//入库记录模型
        public ModifyStorageRecordModel ModifyStorageRecordModel { get; set; }//修改库位记录模型

        #region InputModel
        public string InputOldRFIDNumber
        {
            get { return InputModel.OldRFIDNumber; }
            set
            {
                InputModel.OldRFIDNumber = value;
                this.RaisePropertyChanged("InputOldRFIDNumber");
            }
        }
        public string InputNewRFIDNumber
        {
            get { return InputModel.NewRFIDNumber; }
            set
            {
                InputModel.NewRFIDNumber = value;
                this.RaisePropertyChanged("InputNewRFIDNumber");
            }
        }
        public ComboBoxItem InputCategory
        {
            get { return InputModel.Category; }
            set
            {
                InputModel.Category = value;
                this.RaisePropertyChanged("InputCategory");
            }
        }
        public ComboBoxItem InputAmount
        {
            get { return InputModel.Amount; }
            set
            {
                InputModel.Amount = value;
                this.RaisePropertyChanged("InputAmount");
            }
        }
        #endregion

        #region InputRecordModel

        #endregion

        #region OperationModeModel

        #endregion

        #region OutputRecordModel

        #endregion

        #region StorageInfoModel

        #endregion

        #region UserModel

        #endregion

        #region QueryDataList
        //库位查询数据
        private ObservableCollection<StorageInfoModel> _storagesList = new ObservableCollection<StorageInfoModel>();
        public ObservableCollection<StorageInfoModel> StoragesList
        {
            get { return _storagesList; }
            set
            {
                _storagesList = value;
                this.RaisePropertyChanged("StoragesList");
            }
        }
        //入库记录数据
        private ObservableCollection<InputRecordModel> _inputRecordList = new ObservableCollection<InputRecordModel>();
        public ObservableCollection<InputRecordModel> InputRecordList
        {
            get { return _inputRecordList; }
            set
            {
                _inputRecordList = value;
                this.RaisePropertyChanged("InputRecordList");
            }
        }
        //出库记录数据
        private ObservableCollection<OutputRecordModel> _outputRecordList = new ObservableCollection<OutputRecordModel>();
        public ObservableCollection<OutputRecordModel> OutputRecordList
        {
            get { return _outputRecordList; }
            set
            {
                _outputRecordList = value;
                this.RaisePropertyChanged("OutputRecordList");
            }
        }
        //修改库位记录数据
        private ObservableCollection<ModifyStorageRecordModel> _modifyStorageRecordList = new ObservableCollection<ModifyStorageRecordModel>();
        public ObservableCollection<ModifyStorageRecordModel> ModifyStorageRecordList
        {
            get { return _modifyStorageRecordList; }
            set
            {
                _modifyStorageRecordList = value;
                this.RaisePropertyChanged("ModifyStorageRecordList");
            }
        }
        #endregion

        #region AuxiliaryAttribute
        //切换界面时用
        private ContentControl _contentControl;

        public ContentControl ContentControl
        {
            get { return _contentControl; }
            set
            {
                _contentControl = value;
                this.RaisePropertyChanged("ContentControl");
            }
        }
        //隐藏窗体，显示窗体时用
        private Visibility _gridHomePageIsVisible;

        public Visibility GridHomePageIsVisible
        {
            get { return _gridHomePageIsVisible; }
            set
            {
                _gridHomePageIsVisible = value;
                this.RaisePropertyChanged("GridHomePageIsVisible");
            }
        }
        private Visibility _gridWorkPageIsVisible;

        public Visibility GridWorkPageIsVisible
        {
            get { return _gridWorkPageIsVisible; }
            set
            {
                _gridWorkPageIsVisible = value;
                this.RaisePropertyChanged("GridWorkPageIsVisible");
            }
        }
        //用户名
        public string UserName
        {
            get { return UserModel.UserName; }
            set
            {
                UserModel.UserName = value;
                this.RaisePropertyChanged("UserName");
            }
        }
        //密码
        public string Password
        {
            get { return UserModel.Password; }
            set
            {
                UserModel.Password = value;
                this.RaisePropertyChanged("Password");
            }
        }
        private string _agvCurrentPosition;

        public string AGVCurrentPosition
        {
            get { return _agvCurrentPosition; }
            set
            {
                _agvCurrentPosition = value;
                this.RaisePropertyChanged("AGVCurrentPosition");
            }
        }
        private ComboBoxItem _agvDestination;

        public ComboBoxItem AGVDestination
        {
            get { return _agvDestination; }
            set
            {
                _agvDestination = value;
                this.RaisePropertyChanged("AGVDestination");
            }
        }
        #endregion
        #endregion

        //声明一个传递参数给修改库位信息窗体的一个委托
        public delegate void DeliveryMesseagesEventHandler(int location, string category, int amount);
        //声明一个传递参数给修改库位信息窗体的一个事件
        public event DeliveryMesseagesEventHandler DeliveryMesseagesEvent;

        //构造函数
        public UserViewModel()
        {
            UserModel = new UserModel();//用户模型
            InputModel = new InputModel();//入库模型
            _contentControl = new ContentControl();
            _gridHomePageIsVisible = Visibility.Visible;
            _gridWorkPageIsVisible = Visibility.Hidden;

            InitializeCommand();
            ConnectWMSServer();
        }

        public void InitializeCommand()
        {
            ConfirmOperationModeCommand = new DelegateCommand(new Action(this.ConfirmOperationModeCommandExecute));//操作模式确认

            InputLoadTrayCommand = new DelegateCommand(new Action(this.InputLoadTrayCommandExecute));//入库上料
            ReadRFIDCommand = new DelegateCommand(new Action(this.ReadRFIDCommandExecute));//读取RFID
            WriteRFIDCommand = new DelegateCommand(new Action(this.WriteRFIDCommandExecute));//写入RFID
            InputCommand = new DelegateCommand(new Action(this.InputCommandExecute));//入库执行
            OutputCommand = new DelegateCommand<object>(new Action<object>(this.OutputCommandExecute));//出库执行
            OutputUnloadTrayCommand = new DelegateCommand(new Action(this.OutputUnloadTrayCommandExecute));//出库下料
            CreateRFIDCommand = new DelegateCommand(new Action(this.CreateRFIDCommandExecute));//生成RFID号

            StoragesQueryCommand = new DelegateCommand(new Action(this.StoragesQueryCommandExecute));//库位查询
            InputRecordQueryCommand = new DelegateCommand(new Action(this.InputRecordQueryCommandExecute));//入库记录查询
            OutputRecordQueryCommand = new DelegateCommand(new Action(this.OutputRecordQueryCommandExecute));//出库记录查询
            ModifyStorageRecordQueryCommand = new DelegateCommand(new Action(this.ModifyStorageRecordQueryCommandExecute));//库位修改记录查询
            EditStoragesInfoCommand = new DelegateCommand<object>(new Action<object>(this.EditStoragesInfoCommandExecute));//编辑库位信息

            AGVDispatchGetCurrentPositionCommand = new DelegateCommand(new Action(this.AGVDispatchGetCurrentPositionCommandExecute));//AGV调度：获取当前坐标
            AGVDispatchMoveCommand = new DelegateCommand(new Action(this.AGVDispatchMoveCommandExecute));//AGV调度：移动执行
            AGVDispatchLoadTrayCommand = new DelegateCommand(new Action(this.AGVDispatchLoadTrayCommandExecute));//AGV调度：进料盘任务
            AGVDispatchReleaseTrayCommand = new DelegateCommand(new Action(this.AGVDispatchReleaseTrayCommandExecute));//AGV调度：放料盘任务

        }
        public async void ConnectWMSServer()
        {
            _clientConfig = new TcpSocketSaeaClientConfiguration { FrameBuilder = new LengthPrefixedFrameSlimSlimBuilder() };

            _clientDispatcher = new ClientEventDispatcher();
            _clientDispatcher.ClientStartedEvent += new ClientEventDispatcher.OnServerStartedEventHandler(OnClientStarted);
            _clientDispatcher.ClientDataReceivedEvent += new ClientEventDispatcher.OnServerDataReceivedEventHandler(OnClientDataReceived);
            _clientDispatcher.ClientClosedEvent += new ClientEventDispatcher.OnServerClosedEventHandler(OnClientClosed);

            //the actual debugging
            //private IPEndPoint serverPoint = new IPEndPoint(IPAddress.Parse("192.168.6.3"), 2002);
            //private IPEndPoint localPoint = new IPEndPoint(IPAddress.Parse("192.168.6.100"), 3000);

            //the test debugging
            var serverPoint = new IPEndPoint(IPAddress.Parse("192.168.0.20"), 2002);
            var localPoint = new IPEndPoint(IPAddress.Parse("192.168.0.20"), 3005);

            try
            {
                if (_client != null)
                {
                    _client.Dispose();
                }
                _client = new TcpSocketSaeaClient(serverPoint, localPoint, _clientDispatcher, _clientConfig);
                await _client.Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        #region Command

        #region OtherCommand
        //操作模式选择
        private async void ConfirmOperationModeCommandExecute()
        {
            //操作模式的选择
            OperationModeCommand classOperationModeCommand = new OperationModeCommand();
            classOperationModeCommand.Command = PageHome.OperationMode;
            await _client.SendAsync(DataHelper.SerializeToBinary(classOperationModeCommand));
        }
        #endregion

        #region WarehousePLCCommand
        //入库上料
        private async void InputLoadTrayCommandExecute()
        {
            WarehousePLCInputLoadTrayCommand inputLoadTrayCommand = new WarehousePLCInputLoadTrayCommand();
            inputLoadTrayCommand.InputLoadTrayCommand = "InputLoadTrayCommand";
            await _client.SendAsync(DataHelper.SerializeToBinary(inputLoadTrayCommand));
        }
        //读取RFID
        private async void ReadRFIDCommandExecute()
        {
            WarehousePLCReadRFIDCommand readRFIDCommand = new WarehousePLCReadRFIDCommand();
            readRFIDCommand.ReadRFIDCommand = "ReadRFIDCommand";
            await _client.SendAsync(DataHelper.SerializeToBinary(readRFIDCommand));
        }
        //写入RFID
        private async void WriteRFIDCommandExecute()
        {
            WarehousePLCWriteRFIDCommand writeRFIDCommand = new WarehousePLCWriteRFIDCommand();
            writeRFIDCommand.WriteRFIDCommand = InputNewRFIDNumber.ToString();
            await _client.SendAsync(DataHelper.SerializeToBinary(writeRFIDCommand));
        }
        // 入库执行
        private async void InputCommandExecute()
        {
            WarehousePLCInputExecuteCommand inputExecuteCommand = new WarehousePLCInputExecuteCommand();
            if (InputCategory.Dispatcher.Thread != Thread.CurrentThread)
            {
                InputCategory.Dispatcher.Invoke(new Action(() => { inputExecuteCommand.InputExecuteCommandAmount = Convert.ToInt32(InputAmount.Content.ToString()); }));
                InputCategory.Dispatcher.Invoke(new Action(() => { inputExecuteCommand.InputExecuteCommandCategory = InputCategory.Content.ToString(); }));
                InputCategory.Dispatcher.Invoke(new Action(() => { inputExecuteCommand.InputExecuteCommandRFID = InputNewRFIDNumber; }));
                InputCategory.Dispatcher.Invoke(new Action(() => { inputExecuteCommand.InputExecuteCommandStorageLocation = Convert.ToInt32(InputNewRFIDNumber.Substring(0, 4), 16); }));
            }
            else
            {
                inputExecuteCommand.InputExecuteCommandAmount = Convert.ToInt32(InputAmount.Content.ToString());
                inputExecuteCommand.InputExecuteCommandCategory = InputCategory.Content.ToString();
                inputExecuteCommand.InputExecuteCommandRFID = InputNewRFIDNumber;
                inputExecuteCommand.InputExecuteCommandStorageLocation = Convert.ToInt32(InputNewRFIDNumber.Substring(0, 4), 16);
            }
            await _client.SendAsync(DataHelper.SerializeToBinary(inputExecuteCommand));
        }
        //出库执行
        private async void OutputCommandExecute(object sender)
        {
            if (operationMode == "offline")
            {
                WarehousePLCOutputExecuteCommand outputExecuteCommand = new WarehousePLCOutputExecuteCommand();
                outputExecuteCommand.OutputExecuteCommandStorageLocation = Convert.ToInt32(sender);
                await _client.SendAsync(DataHelper.SerializeToBinary(outputExecuteCommand));
            }
            if (operationMode == "online")
            {
                MessageBox.Show("联机模式下不允许出库！");
            }
        }
        //出库下料
        private async void OutputUnloadTrayCommandExecute()
        {
            WarehousePLCOutputUnloadTrayCommand outputUnloadTrayCommand = new WarehousePLCOutputUnloadTrayCommand();
            outputUnloadTrayCommand.OutputUnloadTrayCommand = "OutputUnloadTrayCommand";
            await _client.SendAsync(DataHelper.SerializeToBinary(outputUnloadTrayCommand));
        }
        //生成新的RFID号
        private void CreateRFIDCommandExecute()
        {
            switch (InputCategory.Content.ToString())
            {
                case "空箱":
                    InputNewRFIDNumber = InputOldRFIDNumber.Substring(0, 4) + "0000";
                    break;
                case "瓶子":
                    InputNewRFIDNumber = InputOldRFIDNumber.Substring(0, 4) + "0101";
                    break;
                case "盖子":
                    InputNewRFIDNumber = InputOldRFIDNumber.Substring(0, 4) + "0201";
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region CloudPlatform
        //库位查询
        private async void StoragesQueryCommandExecute()
        {
            CloudPlatformStorageQueryCommand cloudPlatformStorageQueryCommand = new CloudPlatformStorageQueryCommand();
            cloudPlatformStorageQueryCommand.StorageQueryCommand = 0;
            await _client.SendAsync(DataHelper.SerializeToBinary(cloudPlatformStorageQueryCommand));
        }
        // 入库记录查询
        private async void InputRecordQueryCommandExecute()
        {
            CloudPlatformQueryInputRecordCommand cloudPlatformQueryInputRecordCommand = new CloudPlatformQueryInputRecordCommand();
            cloudPlatformQueryInputRecordCommand.QueryInputRecordCommand = "QueryInputRecordCommand";
            await _client.SendAsync(DataHelper.SerializeToBinary(cloudPlatformQueryInputRecordCommand));
            MessageBox.Show("入库记录查询");
        }
        //出库记录查询
        private async void OutputRecordQueryCommandExecute()
        {
            CloudPlatformQueryOutputRecordCommand cloudPlatformQueryOutputRecordCommand = new CloudPlatformQueryOutputRecordCommand();
            cloudPlatformQueryOutputRecordCommand.QueryOutputRecordCommand = "QueryOutputRecordCommand";
            await _client.SendAsync(DataHelper.SerializeToBinary(cloudPlatformQueryOutputRecordCommand));
            MessageBox.Show("出库记录查询");
        }
        //手动修改库位信息记录查询
        private async void ModifyStorageRecordQueryCommandExecute()
        {
            CloudPlatformQueryModifyStorageRecordCommand cloudPlatformQueryModifyStorageRecordCommand = new CloudPlatformQueryModifyStorageRecordCommand();
            cloudPlatformQueryModifyStorageRecordCommand.QueryModifyStorageRecordCommand = "QueryModifyStorageRecordCommand";
            await _client.SendAsync(DataHelper.SerializeToBinary(cloudPlatformQueryModifyStorageRecordCommand));

        }
        //修改库位信息
        private void EditStoragesInfoCommandExecute(object obj)
        {
            if (operationMode == "offline")
            {
                Button button = obj as Button;
                StorageInfoModel storageInfo = (StorageInfoModel)button.DataContext;
                WndModifyStoragesInfo wndModifyStoragesInfo = new WndModifyStoragesInfo();
                DeliveryMesseagesEvent += wndModifyStoragesInfo.DeliveryMesseagesEventFunc;
                DeliveryMesseagesEvent(storageInfo.Location, storageInfo.Category, storageInfo.Amount);
                wndModifyStoragesInfo.ShowDialog();
            }
            if (operationMode == "online")
            {
                MessageBox.Show("联机模式下不允许修改库位信息");
            }
        }
        /// <summary>
        /// 发送给WMS服务器手动修改库位的消息
        /// </summary>
        /// <param name="location"></param>
        /// <param name="category"></param>
        /// <param name="amount"></param>
        public static async void EditStoragesInfoFunc(int location, string category, int amount)
        {
            CloudPlatformModifyStorageInfoCommand cloudPlatformModifyStorageInfoCommand = new CloudPlatformModifyStorageInfoCommand();
            if (category == "null")
            {
                cloudPlatformModifyStorageInfoCommand.Type = 1;//删除盘子
            }
            else
            {
                cloudPlatformModifyStorageInfoCommand.Type = 2;//更改物料种类
            }
            string tempRFIDNumber = Convert.ToString(location, 16).PadLeft(4, '0');
            switch (category)
            {
                case "bottle":
                    tempRFIDNumber += "0101";
                    break;
                case "lid":
                    tempRFIDNumber += "0201";
                    break;
                case "empty":
                    tempRFIDNumber += "0000";
                    break;
                case "null":
                    tempRFIDNumber = "00000000";
                    break;
            }
            cloudPlatformModifyStorageInfoCommand.Location = location;
            cloudPlatformModifyStorageInfoCommand.CategoryNew = category;
            cloudPlatformModifyStorageInfoCommand.AmountNew = amount;
            cloudPlatformModifyStorageInfoCommand.RFIDNumberNew = tempRFIDNumber;

            await _client.SendAsync(DataHelper.SerializeToBinary(cloudPlatformModifyStorageInfoCommand));
        }
        #endregion

        #region AGVCommand
        //AGV调度：获取当前坐标
        private async void AGVDispatchGetCurrentPositionCommandExecute()
        {
            AGVQueryCurrentPositionCommand classAGVQueryCurrentPositionCommand = new AGVQueryCurrentPositionCommand();
            classAGVQueryCurrentPositionCommand.QueryCurrentPositionCommand = "QueryCurrentPositionCommand";
            await _client.SendAsync(DataHelper.SerializeToBinary(classAGVQueryCurrentPositionCommand));
        }
        //AGV调度：移动任务
        private async void AGVDispatchMoveCommandExecute()
        {
            AGVTaskCommand classAGVTaskCommand = new AGVTaskCommand();
            switch (AGVDestination.Content.ToString())
            {
                case "入库点":
                    classAGVTaskCommand.TaskCommand = 1;
                    break;
                case "出库点":
                    classAGVTaskCommand.TaskCommand = 2;
                    break;
                case "对接平台点":
                    classAGVTaskCommand.TaskCommand = 3;
                    break;
                case "原点":
                    classAGVTaskCommand.TaskCommand = 10;
                    break;
                default:
                    break;
            }
            await _client.SendAsync(DataHelper.SerializeToBinary(classAGVTaskCommand));
        }
        //AGV调度：放料盘任务
        private async void AGVDispatchReleaseTrayCommandExecute()
        {
            AGVTaskCommand classAGVTaskCommand = new AGVTaskCommand();
            classAGVTaskCommand.TaskCommand = 6;
            await _client.SendAsync(DataHelper.SerializeToBinary(classAGVTaskCommand));
        }
        //AGV调度：进料盘任务
        private async void AGVDispatchLoadTrayCommandExecute()
        {
            AGVTaskCommand classAGVTaskCommand = new AGVTaskCommand();
            classAGVTaskCommand.TaskCommand = 7;
            await _client.SendAsync(DataHelper.SerializeToBinary(classAGVTaskCommand));
        }
        #endregion

        #endregion

        #region TcpIP
        private void OnClientStarted(object sender, TcpSocketSaeaClient e)
        {
            MessageBox.Show("start");
        }
        private async void OnClientDataReceived(object sender, ClientDataReceivedEventArgs e)
        {
            byte[] bytes = e._dataBytes;
            string receiveStr = DataHelper.ByteArrayToHexString(bytes);
            //用于确认连接成功的
            if (receiveStr == "10010000")
            {
                await _client.SendAsync(DataHelper.HexStringToByteArray("11010010"));
            }
            else
            {
                Object obj = (Object)DataHelper.DeserializeWithBinary(bytes);
                Type type = obj.GetType();
                //操作模式的反馈
                if (type == typeof(OperationModeResponse))
                {
                    OperationModeResponse operationModeResponse = obj as OperationModeResponse;
                    if (operationModeResponse.Response == "OfflineIsOk")
                    {
                        operationMode = "offline";
                    }
                    if (operationModeResponse.Response == "OnlineIsOk")
                    {
                        operationMode = "online";
                    }
                    //operationMode = operationModeResponse.Response;
                    MessageBox.Show(operationModeResponse.Response);
                }
                //初始化的反馈
                if (type == typeof(InitializeResponse))
                {
                    InitializeResponse initializeResponse = obj as InitializeResponse;

                    MessageBox.Show(initializeResponse.Response);
                }

                #region 来自PLC相关的数据
                //入库上料反馈
                if (type == typeof(WarehousePLCInputLoadTrayResponse))
                {
                    WarehousePLCInputLoadTrayResponse classPLCInputLoadTrayResponse = obj as WarehousePLCInputLoadTrayResponse;

                    MessageBox.Show(classPLCInputLoadTrayResponse.InputLoadTrayResponse);
                }
                //读取RFID的反馈
                if (type == typeof(WarehousePLCReadRFIDResponse))
                {
                    WarehousePLCReadRFIDResponse classPLCReadRFIDResponse = obj as WarehousePLCReadRFIDResponse;
                    InputOldRFIDNumber = classPLCReadRFIDResponse.ReadRFIDResponse;
                }
                //写入RFID的反馈
                if (type == typeof(WarehousePLCWriteRFIDResponse))
                {
                    WarehousePLCWriteRFIDResponse WarehousePLCWriteRFIDResponse = obj as WarehousePLCWriteRFIDResponse;
                    MessageBox.Show(WarehousePLCWriteRFIDResponse.WriteRFIDResponse);
                }
                //入库执行的反馈
                if (type == typeof(WarehousePLCInputExecuteResponse))
                {
                    WarehousePLCInputExecuteResponse classPLCInputExecuteResponse = obj as WarehousePLCInputExecuteResponse;

                    MessageBox.Show(classPLCInputExecuteResponse.InputExecuteResponseStorageLocation.ToString());
                    MessageBox.Show(classPLCInputExecuteResponse.InputExecuteResponseResult);
                }
                //出库执行的反馈
                if (type == typeof(WarehousePLCOutputExecuteResponse))
                {
                    WarehousePLCOutputExecuteResponse WarehousePLCOutputExecuteResponse = obj as WarehousePLCOutputExecuteResponse;
                    MessageBox.Show(WarehousePLCOutputExecuteResponse.OutputExecuteResponseStorageLocation.ToString());
                    MessageBox.Show("出库状态" + WarehousePLCOutputExecuteResponse.OutputExecuteResponseResult);
                }
                #endregion

                #region 来自AGV相关的数据
                if (type == typeof(AGVQueryCurrentPositionResponse))
                {
                    AGVQueryCurrentPositionResponse classAGVQueryCurrentPositionResponse = obj as AGVQueryCurrentPositionResponse;
                    switch (classAGVQueryCurrentPositionResponse.QueryCurrentPositionResponse)
                    {
                        case 1: AGVCurrentPosition = "入库点"; break;
                        case 2: AGVCurrentPosition = "出库点"; break;
                        case 3: AGVCurrentPosition = "对接平台点"; break;
                        case 10: AGVCurrentPosition = "原点"; break;
                        default: break;
                    }
                }
                if (type == typeof(AGVTaskResponse))
                {
                    AGVTaskResponse classAGVTaskResponse = obj as AGVTaskResponse;
                    switch (classAGVTaskResponse.TaskResponse)
                    {
                        case 1: MessageBox.Show("到达入库点！"); break;
                        case 2: MessageBox.Show("到达出库点！"); break;
                        case 3: MessageBox.Show("到达对接平台点！"); break;
                        case 10: MessageBox.Show("回原点完成！"); break;
                        case 6: MessageBox.Show("放料盘完成！"); break;
                        case 7: MessageBox.Show("进料盘完成！"); break;
                        default: break;
                    }
                }
                #endregion

                #region 来自CloudPlatform相关的数据
                //获取所有的库位信息，用于显示
                if (type == typeof(List<CloudPlatformStorageQueryResponse>))
                {
                    List<CloudPlatformStorageQueryResponse> classStorageQueryShowResponses = obj as List<CloudPlatformStorageQueryResponse>;
                    App.Current.Dispatcher.Invoke(() => { _storagesList.Clear(); });//更新新的数据之前先清空列表
                    //List<StorageInfoModel> test = new List<StorageInfoModel>();
                    foreach (var item in classStorageQueryShowResponses)
                    {
                        var list = new StorageInfoModel
                        {
                            Location = item.StorageQueryLocation,
                            Category = item.StorageQueryCategory,
                            Amount = item.StorageQueryAmount,
                            RFIDNumber = item.StorageQueryRFID,
                            EditStoragesInfo = item.StorageQueryCategory != "null",
                            EditOutput = item.StorageQueryCategory != "null"
                        };
                        App.Current.Dispatcher.Invoke(() => { _storagesList.Add(list); });
                    }
                }
                //获取所有的入库记录，用于显示
                if (type == typeof(List<CloudPlatformQueryInputRecordResponse>))
                {
                    List<CloudPlatformQueryInputRecordResponse> classQueryInputRecordResponses = obj as List<CloudPlatformQueryInputRecordResponse>;
                    App.Current.Dispatcher.Invoke(() => { _inputRecordList.Clear(); });//更新新的数据之前先清空列表

                    foreach (var item in classQueryInputRecordResponses)
                    {
                        var list = new InputRecordModel
                        {
                            RFIDNumber = item.QueryInputRecordResponseRFIDNumber,
                            Location = item.QueryInputRecordResponseLocation,
                            Amount = item.QueryInputRecordResponseAmount,
                            OperationTime = item.QueryInputRecordResponseOperationTime,
                            Category = item.QueryInputRecordResponseCategory
                        };
                        App.Current.Dispatcher.Invoke(() => { _inputRecordList.Add(list); });
                    }
                }
                //获取所有的出库记录用于显示
                if (type == typeof(List<CloudPlatformQueryOutputRecordResponse>))
                {
                    List<CloudPlatformQueryOutputRecordResponse> classQueryOutputRecordResponses = obj as List<CloudPlatformQueryOutputRecordResponse>;
                    App.Current.Dispatcher.Invoke(() => { _outputRecordList.Clear(); });//更新新的数据之前先清空列表

                    foreach (var item in classQueryOutputRecordResponses)
                    {
                        var list = new OutputRecordModel
                        {
                            Location = item.QueryOutputRecordResponseLocation,
                            RFIDNumber = item.QueryOutputRecordResponseRFIDNumber,
                            Amount = item.QueryOutputRecordResponseAmount,
                            OperationTime = item.QueryOutputRecordResponseOperationTime,
                            Category = item.QueryOutputRecordResponseCategory
                        };
                        App.Current.Dispatcher.Invoke(() => { _outputRecordList.Add(list); });
                    }
                }
                //获取所有的库位修改记录用于显示
                if (type == typeof(List<CloudPlatformQueryModifyStorageRecordResponse>))
                {
                    List<CloudPlatformQueryModifyStorageRecordResponse> cloudPlatformQueryModifyStorageRecordResponses = obj as List<CloudPlatformQueryModifyStorageRecordResponse>;
                    App.Current.Dispatcher.Invoke(() => { _modifyStorageRecordList.Clear(); });

                    foreach (var item in cloudPlatformQueryModifyStorageRecordResponses)
                    {
                        var list = new ModifyStorageRecordModel
                        {
                            Location = item.Location,
                            ModifyType = item.Type,
                            RFIDNumber = item.RFIDNumber,
                            Category = item.CategoryNew,
                            Amount = item.AmountNew,
                            ModifyTime = item.ModifyTime
                        };

                        App.Current.Dispatcher.Invoke(() => { _modifyStorageRecordList.Add(list); });
                    }
                }
                //手动维护库位信息的反馈
                if (type == typeof(CloudPlatformModifyStorageInfoResponse))
                {
                    CloudPlatformModifyStorageInfoResponse cloudPlatformModifyStorageInfoResponse = obj as CloudPlatformModifyStorageInfoResponse;
                    MessageBox.Show(cloudPlatformModifyStorageInfoResponse.Type.ToString());
                    MessageBox.Show(cloudPlatformModifyStorageInfoResponse.Location.ToString());
                    MessageBox.Show(cloudPlatformModifyStorageInfoResponse.RFIDNumber);
                    MessageBox.Show(cloudPlatformModifyStorageInfoResponse.CategoryNew);
                    MessageBox.Show(cloudPlatformModifyStorageInfoResponse.AmountNew.ToString());
                    MessageBox.Show(cloudPlatformModifyStorageInfoResponse.Result);
                }
                #endregion
            }
        }
        private void OnClientClosed(object sender, TcpSocketSaeaClient session)
        {
            MessageBox.Show("end");
        }
        #endregion
    }
}
