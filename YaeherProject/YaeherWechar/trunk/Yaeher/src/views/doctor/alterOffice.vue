<template>
    <div class="alter-office padding-top">
        <mt-header fixed :title="title">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content add-box">
            <div class="add-case">
                <p class="disease-title">科室</p>
                <!-- <mt-field label="医院：" placeholder="请输入医院名称" v-model="hospital"></mt-field> -->
                <div v-if="id === 0 && clinicID1 === 0" class="illness" @click="selectIllness">
                    <div class="illness-case">
                        <p class="illness-label">选择科室：</p>
                        <p v-if="iIInessType === ''" class="illness-value">请选择科室</p>
                        <p  v-if="iIInessType != ''" class="illness-value">{{iIInessType}}</p>
                    </div>
                </div>
                <div v-if="id != 0 || clinicID1!=0" class="illness">
                    <div class="illness-case">
                        <p class="illness-label">选择科室：</p>
                        <p v-if="iIInessType === ''" class="illness-value">科室</p>
                        <p  v-if="iIInessType != ''" class="illness-value">{{iIInessType}}</p>
                    </div>
                </div>
                
            </div>
            <div v-if="id === 0 && clinicID1 === 0" class="okCase">
                <mt-button @click="goAuthentication" type="primary" class="mint-button--large">下一步</mt-button>
            </div>
            <div v-if="id != 0 || clinicID1!=0 " class="okCase">
                <mt-button @click="goAuthentication" type="primary" class="mint-button--large">去修改</mt-button>
            </div>

            
        </div>
        <mt-picker :slots="dataList" ref="picker" :showToolbar="true" v-show="show">
            <div @click="selectIllness" class="slots-no">取消</div>
            <div @click="getPickerValue" class="slots-ok">确认</div>
        </mt-picker>
    </div>
</template>

<script>
import { Toast } from 'mint-ui';
export default {
    data () {
        return {
            id: 0,
            title: '添加科室',
            clinicID1: 0,
            disease: '',
            hospital: '',
            clinicID: -1,
            iIInessType: '',
            show: false,
            clinicList: [],
            slotsArry: [],
            objList: []
        }
    },
    methods: {
        selectIllness() { // 呼出科室选择
            this.show = !this.show
        },
        getPickerValue() { // 选择科室
            this.show = !this.show
            this.iIInessType = this.$refs.picker.getValues()[0]                
            for (let i = 0; i < this.objList.length; i++) {
                if(this.objList[i].name === this.iIInessType) {
                    this.clinicID = this.objList[i].id
                    return
                } else {
                    this.clinicID = -1
                }
            }
        },
        getUserInfo() { // 请求科室列表
            this.instance.clinicListD({
            })
                .then((response) => {
                    this.clinicList = response.data.result.item
                    for(var j = 0;j < this.clinicList.length;j++ ) {
                        let name = ''
                        let id = -1
                        
                        if (this.clinicList[j].clinicType === 1) {
                            name = `${this.clinicList[j].clinicName}(成人)`
                            id = this.clinicList[j].id
                        } else {
                            name = `${this.clinicList[j].clinicName}(儿童)`
                            id = this.clinicList[j].id
                        }
                        let obj = {
                            name,
                            id
                        }
                        this.slotsArry.push(name)
                        this.objList.push(obj)
                    }
                })
                .catch((error) => {
                }) 
        },
        addOffice () { // 添加科室
            this.instance.doctorClinicApplyD({
            })
                .then((response) => {
					let replyList = response.data.result.item
                    const maxReplyLength = replyList.maxReplyLength
                    window.sessionStorage.setItem('maxReplyLength', maxReplyLength)
                })
                .catch((error) => {
                }) 
        },
        goAuthentication () {
            if (this.clinicID === -1) {
                Toast('请选择科室')
                return
            } else {
                this.$router.push({ 
                    path: '/authentication-clinic',
                    query: {
                        id: this.id,
                        clinicID: this.clinicID
                    }
                })
            }
            
        }
    },
    computed: {
        dataList() {
            let dataSlots = [
                {
                    flex: 1,
                    values: this.slotsArry,
                    className: 'slot1',
                    textAlign: 'center'
                }
            ]
            return dataSlots
        }
    },
    mounted () {
        
        const id = this.$route.query.id
        let clinicID1 = this.$route.query.clinicID
        
        let iIInessType = this.$route.query.iIInessType
        if (id && id != 0) {
            this.id = parseInt(id)
            this.clinicID = parseInt(this.$route.query.clinicID)
            this.title = '修改科室资料'
        } else if (clinicID1) {
            this.clinicID1 = parseInt(clinicID1)
            this.clinicID = parseInt(this.$route.query.clinicID)
            this.title = '修改科室资料'
        } else {
            this.getUserInfo()
        }
        console.log('clinicID1', this.clinicID1)
        if (iIInessType) {
            this.iIInessType = iIInessType
        }
        console.log(this.id)
    },
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.alter-office  {
    .add-box {
        .add-case {
            margin: 10px; 
            padding: 10px 15px; 
            background: $color-wfont;
            p {line-height: 30px;}
        }
        .okCase {
            margin: 20px 10px;
        }
    }
    .illness-case {
        display: flex;
        .illness-label {
            width: 100px;
        }
        .illness-value {
            flex: 1;
        }
    }
}


</style>