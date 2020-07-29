<template>
    <div class="doctor-list-patient">
        <!-- <mt-header fixed :title="title">
            <a v-if="rShow != 'no'" @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header> -->
        <div ref="srcollBox"  class="content listAbout">
            <div class="searchCase">
                <div class="searchBox">
                    <input @keypress="searchKeypress" ref="inputText" type="search" v-model="keyWord" placeholder="请输入想咨询的问题或医生姓名" />
                    <div @click="searchDoctor()">搜索</div>
                </div>
            </div>
            <div class="sort">
                <div v-for="(item, index) in sortList" :key="index" @click="sortSwitch(index)" class="arrowBottom" :class="{arrowTop: item.flag}">{{item.value}}</div>
            </div>
            <div class="diseaseList">
                <span v-for="(item, index) in labelList" :key="index">
                    {{item.lableName}}<span v-if="index != labelList.length-1">、</span>
                </span>
            </div>
            <ul class="group1-list">
                <li v-for="(item, index) in doctorList" :key="index">
                    <router-link :to="{path: '/doctor-detail-patient', query: {id: item.doctorID}}" class="doctor-a">
                        <div class="flex doctorInfo">
                            <img :src="item.userImage | addCompress" />
                            <div class="flexCase">
                                <p class="doctor-name">
                                    {{item.doctorName}} <i>{{item.title}}</i>
                                    <!-- <span v-if="item.serviceState&&item.receiptState">可接单</span> -->
                                    <span v-if="!item.receiptState && item.serviceState">已满额</span>
                                    <span v-if="!item.serviceState">休息中</span>
                                </p>
                                <!-- <p class="star-font"><star :star=4.3></star> <span></span> </p> -->
                                <p class="star-font"><star :star=item.averageEvaluate></star> <span v-if="item.averageEvaluate!=0">{{item.averageEvaluate}}</span> </p>
                                <p>{{item.hospitalName}}</p>
                                <!-- <p>{{item.title}}</p> -->
                                <p class="doctorDis"><span v-for="(item1, index1) in item.doctorslable" :key="index1">
                                    {{item1.lableName}}
                                    <!-- <span v-if="index1 != item.doctorslable.length-1">、</span> -->
                                    </span></p>
                            </div>
                        </div>
                    </router-link>
                </li>
            </ul>        
        </div>
    </div>
</template>

<script>
import Star from 'components/star/star'
import { Toast } from 'mint-ui'
import searchCase from 'components/search/searchCase'
export default {
    components: {
      Star,
      searchCase
    },
    data () {
        return {
            title: '',
            keyWord: '',
            rShow: '',
            homeTop: 0,
            aginId: 0,
            doctorState: 1,
            show: true,
            id: 1,
            skipCount: 1,
            maxResultCount: 10,
            sortType: 'Default',
            sortDesc: 'Asc',
            doctorList: [],
            labelList: [],
            sortList: [
                {
                    flag: false,
                    value: '默认排序'
                },
                {
                    flag: false,
                    value: '价格'
                },
                {
                    flag: false,
                    value: '回复时长'
                },
            ],
            compressHead: '?imageView2/q/20'
        }
    },
    filters: {
        addCompress (value) {
            let str1 = `${value.replace(/cos.ap-guangzhou/g, 'picgz')}?imageView2/q/20`
            return str1
        }
    },
    methods: {
        sortSwitch(index) { // 排序点击
            this.$indicator.open({
                text: '排序中，请稍候',
                spinnerType: 'fading-circle'
            })
            for (let i = 0; i < this.sortList.length; i++) {
                if(index === i) {
                    this.sortList[i].flag = !this.sortList[i].flag
                    if(this.sortList[i].flag) {
                        this.sortDesc = 'Asc'
                    } else {
                        this.sortDesc = 'Desc'
                    }
                } else {
                    this.sortList[i].flag = false
                }
            }
            let array = ['Default', 'Expense', 'AnswerTimer']
            this.sortType = array[index]
            this.getYaeherClinicDoctors()
        },
        getYaeherClinicDoctors() { // 获取医生列表
            const clinliId = this.id
            const sortType = this.sortType
            const sortDesc = this.sortDesc
            this.instance.yaeherClinicDoctors({
                clinliId,
                sortType,
                sortDesc
            })
                .then((response) => {
                    this.$indicator.close()
                    this.doctorList = response.data.result.item
                })
                .catch((error) => {
                    console.log('专业分组请求失败')
                    this.$indicator.close()
                }) 
        },
        getLableList() { // 获取科室对应标签
            const clinicID = this.id
            this.instance.lableList({
                clinicID,
            })
                .then((response) => {
                    this.labelList = response.data.result.item
                })
                .catch((error) => {
                    console.log('科室对应标签')
                }) 
        },
        searchKeypress (event) {
            if (event.keyCode == 13) {
                this.searchDoctor()
            } 
        },
        searchDoctor() { // 搜索医生
            const keyWord = this.keyWord
            const clinicID = this.id
            this.$refs.inputText.blur()
            this.instance.doctorSearch({
                clinicID,
                keyWord,
                onlineState: 'online'
            })
                .then((response) => {
                    this.doctorList = response.data.result.item.items
                    for (var i = 0 ; i < this.doctorList.length; i++) {
                        this.$set(this.doctorList[i], 'doctorID', this.doctorList[i].id)
                    }
                })
                .catch((error) => {
                }) 
            
        }
    },
    mounted () {
        
    },
    created () {
        
    },
    activated () {
        this.doctorList = []
        this.labelList = []
        
        let id = this.$route.query.id
		if (id) {
            this.id = parseInt(id)
            this.rShow = this.$route.query.rShow
            this.title = this.$route.query.clinicNmae
            document.title = this.title
		}
        this.getLableList()
        this.getYaeherClinicDoctors()
        
    },
    beforeRouteLeave (to, from, next) {
        let scrollA = this.$refs.srcollBox
        this.homeTop = scrollA.scrollTop || 0
        next()
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.doctor-list-patient {
    .searchCase {
        padding: 10px;
    }
    .listAbout {
        position: absolute; left: 0; right: 0; bottom: 0px;top: 0px;
        overflow: auto;
        -webkit-overflow-scrolling: touch;
    }
    .give-star .starNo {
        width: 12px;
        height: 12px;
    }
    .give-star .starOk {
        width: 12px;
        height: 12px;
    }
    .give-star .starBan {
        width: 12px;
        height: 12px;
    }
    .sort {display: flex; background: $color-wfont; margin: 10px 0 0 0;}
    .sort > div {flex: 1; color: $color-afont; font-size: $font-l; line-height: 40px; text-align: center; border-right: 1px solid #ccc; position: relative;}
    .sort .arrowBottom:after{ 
        content: '';
        width:0;
        height:0;
        vertical-align: bottom;
        margin-left: 5px;
        line-height: 19px;
        border-width:5px 5px 0;
        border-style:solid;
        border-color: $color-afont transparent transparent;}

    .sort .arrowTop:after{ 
        content: '';
        width:0;
        height:0;
        vertical-align: top;
        margin-left: 5px;
        line-height: 19px;
        border-width: 0px 5px 5px;
        border-style:solid;
        border-color: transparent transparent $color-afont;}    
    .diseaseList {margin-top: 10px; background: #e8edfe; color: $color-afont; padding: 15px 15px; font-size: $font-l;}  
    .group1-list { 
        font-size: $font-m;
        color: $color-bfont;
        background: $color-wfont;
        li {
            padding: 20px 0; 
            display: flex;
            .doctor-a {
                display: block;
                width: 100%;
                .doctorInfo {
                    img{
                        display: block;
                        margin-top: 3px;
                        width: 65px;
                        height: 65px;
                        border-radius: 5px;
                    }
                    .flexCase {
                        padding: 0 10px; 
                        p {
                            font-size: $font-m;
                            color: $color-bfont;
                            padding-bottom: 4px;
                            overflow:hidden;
                            width: 250px;
                        }
                        .doctorDis {
                            height: 25px;
                            padding-top: 2px;
                            span{
                                display: inline-block; 
                                margin-right: 10px; 
                                margin-bottom: 8px;
                                padding: 5px 4px; 
                                font-size: 12px; 
                                color: $color-bfont; 
                                line-height: 14px;
                                height: 12px; 
                                border-radius: 10px; 
                                border: 1px solid #ddd;
                            }
                        }
                        .doctor-name {
                            font-size: $font-xl;
                            color: $color-afont;
                            width: 100%;
                            i {
                                display: inline-block;
                                padding-left: 5px;
                                font-style: normal;
                                font-size: $font-m;
                                color: #666;
                            }
                            span {
                                color: $color-star;
                                font-size: $font-m;
                                float: right;
                            }
                        }
                        .star-font {
                            color: $color-star;
                            .give-star {
                                padding-left: 0;
                            }
                        }
                    }
                }  
            }
        }
    }
}


</style>