<template>
    <div class="control-doctor-list padding-top">
        <mt-header fixed :title="title">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content">
            <div class="searchCase">
                <div class="searchBox">
                    <input @keypress="searchKeypress" type="search" ref="inputText" v-model="keyWord" placeholder="请输入医生的名字或者标签" />
                    <div @click="searchDoctor">搜索</div>
                </div>
            </div>
            <ul class="group1-list">
                <li v-for="(item, index) in doctorList" :key="index">
                    <a class="doctor-a">
                        <div class="doctorInfo">
                            <router-link v-if="id!=0" class="imgWidth" tag="div" :to="{path: '/doctor-detail-patient', query: {id: item.doctorID}}">
                                <img :src="item.userImage" />
                            </router-link>
                            <router-link v-if="id===0" class="imgWidth" tag="div" :to="{path: '/doctor-detail-patient', query: {id: item.id}}">
                                <img :src="item.userImage" />
                            </router-link>
                            <div class="flexCase">
                                <router-link v-if="id!=0" tag="div" :to="{path: '/order-list-control', query: {id: item.doctorID}}" class="clickOrder">订单查看</router-link>
                                <router-link v-if="id===0" tag="div" :to="{path: '/order-list-control', query: {id: item.id}}" class="clickOrder">订单查看</router-link>
                                <span class="timeCreate">{{item.registerDate}}</span>
                                <p class="doctor-name">
                                    {{item.doctorName}}
                                </p>
                                <p class="star-font"><star :star=item.averageEvaluate></star> {{item.averageEvaluate}}</p>
                                <p>{{item.hospitalName}}</p>
                                <p>{{item.title}}</p>
                                <p><span v-for="(item1, index1) in item.doctorslable" :key="index1">{{item1.lableName}}<span v-if="index1 != item.doctorslable.length-1">、</span></span></p>
                            </div>
                        </div>
                    </a>
                </li>
            </ul>        
        </div>
    </div>
</template>

<script>
import Star from 'components/star/star'
import { Toast } from 'mint-ui'
import searchCase from 'components/search/searchCase'
let moment = require('moment');
export default {
    components: {
      Star,
      searchCase
    },
    data () {
        return {
            keyWord: '',
            doctorState: 1,
            id: 0,
            skipCount: 1,
            maxResultCount: 20,
            title: '医生搜索',
            doctorList: [],
            labelList: [],
            new: '',
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
            ]
        }
    },
    methods: {
        getYaeherClinicDoctors1() { // 获取科室医生列表
            const clinliId = this.id
            const keyWord = this.keyWord
            this.instance.yaeherClinicDoctors({
                clinliId,
                keyWord,
                sortType: 'Default',
                sortDesc: 'Asc'
            })
                .then((response) => {
                    this.doctorList = response.data.result.item
                    for (let i = 0; i < this.doctorList.length; i++) {
                        this.doctorList[i].registerDate = moment(this.doctorList[i].registerDate).format('YYYY-MM-DD HH:mm:ss')
					}
                })
                .catch((error) => {
                }) 
        },
        getYaeherClinicDoctors() { // 查询科室医生
            const clinicID = this.id
            const keyWord = this.keyWord
            this.instance.doctorSearch({
                clinicID,
                keyWord,
                onlineState: ''
            })
                .then((response) => {
                    this.doctorList = response.data.result.item.items
                    for (let i = 0; i < this.doctorList.length; i++) {
                        this.doctorList[i].createdOn = moment(this.doctorList[i].createdOn).format('YYYY-MM-DD HH:mm:ss')
					}
                })
                .catch((error) => {
                }) 
        },
        getnewDoctor() { // 获取新医生列表
            const keyWord = this.keyWord
            this.instance.qualityYaeherDoctorSearchC({
                keyWord
            })
                .then((response) => {
                    this.doctorList = response.data.result.item.items
                    for (let i = 0; i < this.doctorList.length; i++) {
                        this.doctorList[i].createdOn = moment(this.doctorList[i].createdOn).format('YYYY-MM-DD HH:mm:ss')
					}
                })
                .catch((error) => {
                    console.log('获取新医生失败')
                }) 
        },
        searchKeypress (event) {
            if (event.keyCode == 13) {
                this.searchDoctor()
            } 
        },
        searchDoctor() { // 搜索医生
            const keyWord = this.keyWord
            this.$refs.inputText.blur()
            if (this.new){
                this.getnewDoctor()
            } else {
                this.getYaeherClinicDoctors()
            }
            
        }
    },
    mounted () {
        const keyWord = this.keyWord
            
        if (this.new){
            this.getnewDoctor()
            return
        }
        if(this.id) {
            this.getYaeherClinicDoctors1()
            return
        }
        if (keyWord) {
            this.getYaeherClinicDoctors()
            return
        }
    },
    created () {
        let id = this.$route.query.id
        if(id) this.id = parseInt(id)
        
        let title = this.$route.query.clinic
        if (title) this.title = title

        this.new = this.$route.query.new
        this.keyWord = this.$route.query.keyWord
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.control-doctor-list {
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
            padding: 15px 0; 
            display: flex;
            .doctor-a {
                display: block;
                width: 100%;
                .doctorInfo {
                    display: flex;
                    .imgWidth {
                        display: block;
                        width: 60px;
                        height: 60px;
                        border-radius: 5px;
                        img {
                            display: block;
                            width: 60px;
                            height: 60px;
                            border-radius: 5px;
                        }
                    }
                    .flexCase {
                        padding: 0 10px; 
                        flex: 1;
                        position: relative;
                        .clickOrder {
                            position: absolute;
                            top: 0;
                            right: 0;
                            display: block;
                            width: 60px;
                            height: 30px;
                            line-height: 30px;
                            background: $default-color;
                            color: #fff;
                            text-align: center;
                            border-radius: 3px;
                        }
                        .timeCreate {
                            position: absolute;
                            top: 35px;
                            right: 0;
                            display: block;
                            height: 30px;
                        }
                        p {
                            font-size: $font-m;
                            color: $color-bfont;
                            width: 230px;
                            padding-bottom: 2px;
                            white-space: nowrap;
                            overflow: hidden;
                            text-overflow: ellipsis;
                        }
                        .doctor-name {
                            font-size: $font-xl;
                            color: $color-afont;
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