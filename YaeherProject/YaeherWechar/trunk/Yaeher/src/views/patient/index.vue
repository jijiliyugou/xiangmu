<template>
    <div class="index-patient">
        <div ref="srcollBox" class="p-content listAbout">
            <search-case></search-case>
            <div class="swipeBox">
                <mt-swipe :auto="3000" :speed="500">
                    <mt-swipe-item v-for="(item, index) in bannerList" :key="index">
                        <a :href="item.bannerUrl">
                            <img :src="item.bannerImageUrl">
                        </a>
                    </mt-swipe-item>
                </mt-swipe>
            </div>
            <!-- <div class="professional-group"> -->
                <!-- <div class="listTitle relative">
                    专业分组
                </div> -->
                <mt-loadmore v-show="listShow" :bottom-method="loadBottomUse"
                    :bottom-all-loaded="allUseLoad" :bottomPullText='bottomText' :auto-fill="false"
                    ref="loadmore">
                    <ul class="group-list">
                        <li v-for="(item, index) in clinicDoctors" :key="index">
                            <router-link class="" :to="{path: '/doctor-list-patient', query: {id: item.id, clinicNmae: item.clinicName}}">
                                <p class="group-title">{{item.clinicName}}</p>
                                <p class="lableCase">
                                    <span v-for="(item1, index1) in item.lableManages" :key="index1">
                                        {{item1.lableName}}<span v-if="index1 != item.lableManages.length-1">、</span>
                                    </span>
                                </p>
                                <div class="group-case">
                                    <span class="avaterNub">{{item.doctorCount}}</span>
                                    <i class="mint-cell-allow-right"></i>
                                    <img v-if="index2<=5" v-for="(item2, index2) in item.doctorDetailList" :key="index2"  :src="item2.userImage| addCompress" />
                                </div>
                            </router-link>
                        </li>
                    </ul>
                </mt-loadmore>
            
                <mt-loadmore v-show="!listShow" :bottom-method="loadBottomUse1"
                    :bottom-all-loaded="allUseLoad1" :bottomPullText='bottomText' :auto-fill="false"
                    ref="loadmore1">
                    <ul class="group-list">
                        <li v-for="(item, index) in clinicDoctors1" :key="index">
                            <router-link class="" :to="{path: '/doctor-list-patient', query: {id: item.id, clinicNmae: item.clinicName}}">
                                <p class="group-title">{{item.clinicName}}</p>
                                <p class="lableCase">
                                    <span v-for="(item1, index1) in item.lableManages" :key="index1">
                                        {{item1.lableName}}<span v-if="index1 != item.lableManages.length-1">、</span>
                                    </span>
                                </p>
                                <div class="group-case">
                                    <span class="avaterNub">{{item.doctorCount}}</span>
                                    <i class="mint-cell-allow-right"></i>
                                    <img v-if="index2<=5" v-for="(item2, index2) in item.doctorDetailList" :key="index2" :src="item2.userImage| addCompress" />
                                </div>
                            </router-link>
                        </li>
                    </ul>
                </mt-loadmore>
            </div>
            <!-- <div v-if="false" class="fixedR">
                <router-link to="chat">联系客服</router-link>
            </div> -->
        <!-- </div> -->
        <ul class="navBar">
            <li class="childrenIcon" @click="navSelect(true)">
                <a :class="{ 'nav-active': isActive }" class="mint-tab-item is-selected">
                    <div class="mint-tab-item-icon">
                        <img v-show="isActive" src="../../assets/image/childrenIcon1.png"/>
                        <img v-show="!isActive" src="../../assets/image/childrenIcon.png"/>
                    </div> 
                    <div class="mint-tab-item-label">
                    儿童
                    </div>
                </a>
            </li>
            <li  class="childrenIcon" @click="navSelect(false)">
                <a :class="{ 'nav-active': !isActive }" class="mint-tab-item is-selected">
                    <div class="mint-tab-item-icon">
                        <img v-show="!isActive" src="../../assets/image/adultIcon1.png"/>
                        <img v-show="isActive" src="../../assets/image/adultIcon.png"/>
                    </div> 
                    <div class="mint-tab-item-label">
                    成人
                    </div>
                </a>
            </li>
            <!-- <li class="childrenIcon" @click="navSelect(true)"><a :class="{ 'nav-active': isActive }" href="javascript:;"><i></i><span>儿童</span></a></li>
            <li class="adultIcon" @click="navSelect(false)"><a :class="{ 'nav-active': !isActive }" href="javascript:;"><i></i><span>成人</span></a></li> -->
        </ul>
        <!-- <mt-tabbar v-model="selected" fixed>
            <mt-tab-item id="children" @click="navSelect(true)">
                <img slot="icon" src="../../assets/image/childrenIcon.png">
                儿童
            </mt-tab-item>
            <mt-tab-item id="adult" @click="navSelect(false)">
                <img slot="icon" src="../../assets/image/adultIcon.png">
                成人
            </mt-tab-item>
        </mt-tabbar> -->
    </div>
</template>

<script>
import searchCase from 'components/search/searchCase'
import { Tabbar, TabItem, Toast } from 'mint-ui';
export default {
    components: {
      searchCase
    },
    data () {
        return {
            searchValue: '',
            isActive: true,
            clinicType: 2,
            skipCount: 1,
            skipCount1: 1,
			maxResultCount: 10,
            totalPage: 2,
            totalPage1: 2,
            clinicDoctors: [],
            clinicDoctors1: [],
            bannerList: [],
            userImage: '',
            loading: true,
            homeTop: 0,
            selected: 'children',
            listShow: true,
            allUseLoad: false,
            allUseLoad1: false,
            bottomText: '上拉加载更多...',
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
        navSelect(flag) {
            if (this.isActive === flag) return
            this.isActive = flag
            this.listShow = flag
            // alert(this.$refs.srcollBox.scrollTop)
            // alert(document.body.scrollTop)
            // this.$refs.srcollBox.scrollTop = 0
            document.body.scrollTop = 0
            
        },
        getUserInfo() { // 请求个人信息
            this.instance.patientInfo({
            })
                .then((response) => {
                    this.userImage = response.data.result.item.userImage
                    const userId = response.data.result.item.id
                    window.sessionStorage.setItem('userId', userId)

                })
                .catch((error) => {
                }) 
        },
        getClinicDoctorsPage(nub) { // 请求专业分组
            let _this = this
            const clinicType = nub
            const maxResultCount = this.maxResultCount
            let skipCount = 0
            let totalPage = 0
            if (nub === 1) {
                skipCount = this.skipCount1
                totalPage = this.totalPage1
            } else {
                skipCount = this.skipCount
                totalPage = this.totalPage
            }
            this.instance.clinicDoctorsPage({
                skipCount,
                clinicType,
                maxResultCount
            })
                .then((response) => {
                    this.$indicator.close()
                    this.$refs.loadmore.onBottomLoaded()
                    this.$refs.loadmore1.onBottomLoaded()
                    if (response.data.result.code === 200) {
                        let clinicDoctors2 = response.data.result.item.items
                        if (clinicType === 1) {
                            _this.clinicDoctors1 = _this.clinicDoctors1.concat(clinicDoctors2)
                            _this.totalPage1 = response.data.result.item.totalPage
                        } else {
                            _this.clinicDoctors = _this.clinicDoctors.concat(clinicDoctors2)
                            _this.totalPage = response.data.result.item.totalPage
                        }
                    }
                })
                .catch((error) => {
                    this.$indicator.close()
                    this.$refs.loadmore.onBottomLoaded()
                    this.$refs.loadmore1.onBottomLoaded()
                }) 
        },
        loadBottomUse() {
            this.skipCount += 1;
            if (this.skipCount >= this.totalPage) {
                this.allUseLoad = true;
            }
            setTimeout(() => {
                this.getClinicDoctorsPage(2)
            }, 500);
        },
        loadBottomUse1() {
            this.skipCount1 += 1;
            if (this.skipCount1 >= this.totalPage1) {
                this.allUseLoad1 = true;
            }
            setTimeout(() => {
                this.getClinicDoctorsPage(1)
            }, 500);
        },
        getBannerList() { // 请求轮播图数据
            this.instance.yaeherBannerList({
            })
                .then((response) => {
                    this.bannerList = response.data.result.item
                })
                .catch((error) => {
                }) 
        }
    },
    mounted () {
        this.getBannerList()
        this.getClinicDoctorsPage(2)
        this.getClinicDoctorsPage(1)     
    },
    created () {
        this.$indicator.open({
            text: '加载中，请稍候',
            spinnerType: 'fading-circle'
        })
    },
    activated () {
        document.title = '怡禾健康'
        this.$refs.srcollBox.scrollTop = this.homeTop || 0
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

.index-patient {
    position: relative;
	height: 100%;
    .listAbout {
        position: absolute;
		left:0;
		top: 0;
		width:100%;
		overflow:auto;
		padding: 0px 0 70px 0;
		box-sizing:border-box;
		-webkit-overflow-scrolling: touch;
    }
    .navBar {
        height: 55px;
        .mint-tab-item-icon {
            padding-right: 2px;
        }
    }
    // .navBar li a {
    //     text-align: center;
    //     .childrenIcon {

    //     }
    // }
    .topCase {
        background: $default-color;
        overflow: hidden;
        padding: 10px 15px;
        img {
            width: 30px;
            height: 30px;
            border-radius: 50%;
        }
        .logo {
            float: left;
        }
        .avatar {
            float: right;
        }

    }
    .swipeBox{
        height: 180px;
        background-color: $default-color;
        img {
            width: 100%;
            height: 100%;
        }
        .mint-swipe-indicator {
            background: #fff;
            opacity: 0.8;
        }
        .mint-swipe-indicator.is-active {
            background: $default-color;
            opacity: 0.8;
        }
    }
    .searchCase {
        padding: 10px;
    }
    
    .company-info {
        width: 100%;
        height: 250px;
        background: $default-color;
        color: $color-wfont;
        text-align: center;
        .logo { 
            padding: 50px 0 10px; 
            display: flex; 
            justify-content: center; 
            align-items: center; 
            position: relative;
            img {
                width: 50px; 
                height: 50px; 
                display: block; 
                border-radius: 10px; 
            }
            .avatar{
                width: 30px; 
                height: 30px; 
                border-radius: 50%; 
                position: absolute; 
                top: 20px; 
                right: 20px;
                img {
                    width: 100%; 
                    height: 100%; 
                    border-radius: 50%;
                }
            }
        }
        .logoTitle {
            padding: 0 20px;
            h3 { 
                padding: 10px 0;
            }
            p {
                text-align: left; 
                font-size: $font-m;
            }
        }
    }

    .nav-active {color: $default-color; background-color: #fafafa;}
    .listTitle {font-size: $font-l; padding: 10px 20px;}

    .group-list { padding: 0 15px;
        font-size: $font-m;
        color: $color-bfont;
        background: $color-wfont;
        li {
            padding: 15px 0; 
            p {
                padding-bottom: 5px;
            }
            .lableCase {
                overflow : hidden;
                text-overflow: ellipsis;
                display: -webkit-box;
                -webkit-line-clamp: 2;
                -webkit-box-orient: vertical;
                padding-bottom: 0;
                margin-bottom: 5px; 
            }
            .group-title {
                font-size: $font-l;
                color: $color-afont;
                font-weight: 700;
            }
            .group-case {
                background: #e8edfe;
                height: 30px;
                padding: 10px;
                overflow: hidden;
                border-radius: 5px;
                position: relative;
                img {
                    float: left;
                    width: 30px;
                    height: 30px;
                    margin-right: 5px;
                    border-radius: 3px;
                }    
                .avaterNub {
                    position: absolute;
                    top: 20px;
                    right: 35px;
                    color: $color-cfont;
                }
            }
        }
    }

    .fixedR {
        font-size: $font-l; 
        color: $color-afont; 
        background: $color-wfont; 
        position: fixed; 
        width: 15px;
        padding: 10px;
        top: 100px; 
        right: 0px; 
        z-index: 1000;
        border-top-left-radius: 3px; 
        border-bottom-left-radius: 3px;
        a {
            display: block; 
            width: 100%; 
            height: 100%; 
            color: #333;
        }
    }
}


</style>
